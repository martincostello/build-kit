// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Reflection;

namespace MartinCostello.BuildKit.NuGetPackage;

/// <summary>
/// A class containing metadata for the NuGet package. This class cannot be inherited.
/// </summary>
public static class PackageMetadata
{
    /// <summary>
    /// Gets the Id for the GitHub Actions run the package was compiled from.
    /// </summary>
    public static string BuildId { get; } = GetMetadataValue("BuildId", string.Empty);

    /// <summary>
    /// Gets the branch that the package was built from.
    /// </summary>
    public static string Branch { get; } = GetMetadataValue("CommitBranch", "Unknown");

    /// <summary>
    /// Gets the Git commit hash that the package was built from.
    /// </summary>
    public static string Commit { get; } = GetMetadataValue("CommitHash", "HEAD");

    /// <summary>
    /// Gets the value for a piece of assembly metadata that does not exist.
    /// </summary>
    public static string DoesNotExist { get; } = GetMetadataValue("DoesNotExist", string.Empty);

    /// <summary>
    /// Gets the date and time that the package was built.
    /// </summary>
    public static DateTimeOffset Timestamp { get; } = DateTimeOffset.Parse(GetMetadataValue("BuildTimestamp", DateTimeOffset.UtcNow.ToString("u", CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets the version of the package.
    /// </summary>
    public static string Version { get; } = typeof(PackageMetadata).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

    private static string GetMetadataValue(string name, string defaultValue)
    {
        return typeof(PackageMetadata).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Where((p) => string.Equals(p.Key, name, StringComparison.Ordinal))
            .Select((p) => p.Value)
            .FirstOrDefault() ?? defaultValue;
    }
}
