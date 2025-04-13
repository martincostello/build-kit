// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace MartinCostello.BuildKit;

public abstract class IntegrationTests(ITestOutputHelper outputHelper)
{
    private static readonly string SolutionRoot =
        typeof(IntegrationTests).Assembly
        .GetCustomAttributes()
        .OfType<AssemblyMetadataAttribute>()
        .FirstOrDefault((p) => p.Key is "SolutionRoot")?.Value ?? string.Empty;

    protected async Task<IReadOnlyDictionary<string, string?>> EvaluatePropertyAsync(
        string name,
        IReadOnlyDictionary<string, string?>? properties = null,
        CancellationToken cancellationToken = default)
        => await EvaluateProperties([name], properties, cancellationToken);

    protected async Task<IReadOnlyDictionary<string, string?>> EvaluateProperties(
        IEnumerable<string> names,
        IReadOnlyDictionary<string, string?>? properties = null,
        CancellationToken cancellationToken = default)
    {
        var target = Path.Combine(SolutionRoot, "src", "BuildKit", "build", "MartinCostello.BuildKit");
        using var project = new TemporaryProjectFile();

        var content =
            $"""
             <Project Sdk="Microsoft.NET.Sdk">
               <Import Project="{target}.props" />
               <Import Project="{target}.targets" />
               <PropertyGroup>
                 <OutputType>Library</OutputType>
                 <TargetFramework>net{Environment.Version.ToString(2)}</TargetFramework>
               </PropertyGroup>
             </Project>
             """;

        await File.WriteAllTextAsync(project.FileName, content, cancellationToken);

        var arguments = new List<string>()
        {
            "msbuild",
            project.FileName,
            $"-getProperty:{string.Join(',', names)}",
        };

        if (properties is not null)
        {
            foreach ((var name, var value) in properties)
            {
                arguments.Add($"-property:{name}={value}");
            }
        }

        var startInfo = new ProcessStartInfo("dotnet", arguments)
        {
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        using var msbuild = Process.Start(startInfo);
        msbuild.ShouldNotBeNull();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);

        await msbuild.WaitForExitAsync(linked.Token);

        var stdout = await msbuild.StandardOutput.ReadToEndAsync(linked.Token);
        var stderr = await msbuild.StandardError.ReadToEndAsync(linked.Token);

        if (stdout is { Length: > 0 })
        {
            outputHelper.Write(stdout);
        }

        if (stderr is { Length: > 0 })
        {
            outputHelper.Write(stderr);
        }

        msbuild.ExitCode.ShouldBe(0);

        var result = new Dictionary<string, string?>();

        using var document = JsonDocument.Parse(stdout);

        if (document.RootElement.TryGetProperty("Properties", out var props))
        {
            foreach (var property in props.EnumerateObject())
            {
                if (property.Value.ValueKind is JsonValueKind.String)
                {
                    result[property.Name] = property.Value.GetString();
                }
            }
        }

        return result;
    }

    private sealed class TemporaryProjectFile : IDisposable
    {
        public TemporaryProjectFile()
        {
            var temp = Path.GetTempFileName();
            FileName = Path.ChangeExtension(temp, ".csproj");
            File.Move(temp, FileName, true);
        }

        public string FileName { get; }

        public void Dispose()
        {
            try
            {
                File.Delete(FileName);
            }
            catch (Exception)
            {
                // Ignore
            }
        }
    }
}
