# MartinCostello.BuildKit 🛠️

[![NuGet][package-badge-version]][package-download]
[![NuGet Downloads][package-badge-downloads]][package-download]

[![Build status][build-badge]][build-status]
[![codecov][coverage-badge]][coverage-report]

[![OpenSSF Scorecard][scorecard-badge]][scorecard-report]

## Introduction

A NuGet package containing reusable MSBuild properties, items and targets for building and deploying my other .NET projects.

The functionality provided is very opinionated, even though it provides the ability to customize the build process.

As such, is not intended to be used by other people other than myself - but you're welcome to take inspiration from it!

## Installation

To install the [NuGet package][nuget-package], it is recommended to use a global package reference
in your `Directory.Packages.props` file as shown below:

```xml
<Project>
  <ItemGroup>
    <GlobalPackageReference Include="MartinCostello.BuildKit" Version="0.1.2" />
  </ItemGroup>
</Project>
```

## Features

At a high-level, the package provides the following features:

- Configures warnings as errors.
- Configures .NET and code style analysis.
- Enables C# implicit using statements and nullable reference types.
- Configures reproducible builds.
- Simplifies the configuration of strong naming.
- Adds more default global using statements.
- Embeds additional Git metadata in assemblies.
- Installs/restores npm packages.
- Configures versions plus container and package metadata for builds that take place in GitHub Actions.
- Configures code coverage using when using VSTest (with [Coverlet][coverlet]) or [Microsoft.Testing.Platform][mtp].
- Generate code coverage reports using [ReportGenerator][reportgenerator].
- Assumes the use of [artifacts output][artifacts-output]

## MSBuild Documentation

A summary of the MSBuild properties, items and targets provided by the package is provided below.

For more detailed information, consult the `.props` and `.targets` files [here][source].

### Properties

The following custom properties are provided to control behaviour.

It is recommended to set these values in `Directory.Build.props` (or the `.csproj` file of a single project).

| **Property** | **Default Value** | **Description** |
|--------------|-----------------|-------------------|
| `AssemblyIsCLSCompliant` | `true` (`false` for test projects) | Emits the `[CLSCompliant]` attribute with the specified value |
| `CoverageFormat` | `cobertura` | The coverage output format to use with Microsoft.Testing.Platform  |
| `CoverageOutput` | `artifacts/coverage/{project name}/coverage.xml` | The coverage output file to use with Microsoft.Testing.Platform |
| `CoverageOutputPath` | `artifacts/coverage` | The path to write code coverage results to |
| `CoverageRunSettings` | - | The path to the `.runsettings` file to use with Microsoft.Testing.Platform |
| `GenerateGitMetadata` | `true` | Whether to embed additional `[AssemblyMetdata]` for Git projects |
| `TestResultsDirectory` | `artifacts/tests` | The path to write test logs to with Microsoft.Testing.Platform |
| `UseDefaultAssemblyOriginatorKeyFile` | `false` | Whether to use the built-in `.snk` file |
| `UseDefaultCodeAnalysisRuleSet` | `false` | Whether to use the built-in `.ruleset` file |
| `UseDefaultTestRunSettings` | `true` | Whether to use the built-in `.runsettings` file with Microsoft.Testing.Platform |

The following properties are made available for use in the build process:

| **Property** | **Description** |
|--------------|-----------------|
| `DynamicProxyGenAssembly2StrongNamePublicKey` | The public key to use with `[InternalsVisibleTo]` for DynamicProxyGenAssembly2 when using NSubstitute or Moq |
| `GitHubBranchName` | The name of the current Git branch when not a pull request |
| `GitHubIsPullRequest` | Whether the current build was triggered by a pull request |
| `GitHubIsTag` | Whether the current build was triggered by a tag |
| `GitHubPullRequest` | The number of the pull request that triggered the current build, if any |
| `GitHubRepositoryUrl` | The URL of the GitHub repository |
| `GitHubTag` | The value of the tag that triggered the current build, if any |
| `IsGitHubActions` | Set to `true` when running in GitHub Actions |
| `StrongNamePublicKey` | When `UseDefaultAssemblyOriginatorKeyFile=true` contains the public key for the `.snk` file |

### Items

The following custom items are provided:

| **Property** | **Defaults** | **Description** |
|--------------|--------------|-----------------|
| `CoverletExclude` | Tests and xunit | Patterns to exclude from code coverage when using Coverlet (ensure values are [escaped][msbuild-escape]) |
| `CoverletExcludeByAttribute` | `GeneratedCodeAttribute` | Patterns for attributes to exclude from code coverage when using Coverlet |
| `CoverletExcludeByFile` | `artifacts/obj/*` | Patterns for files to exclude from code coverage when using Coverlet (ensure values are [escaped][msbuild-escape]) |
| `CoverletInclude` | - | Patterns to include in code coverage when using Coverlet (ensure values are [escaped][msbuild-escape]) |
| `CoverletOutputFormats` | `cobertura` and `json` | The output formats to use with Coverlet |
| `ReportGeneratorReportTypes` | `HTML` | The report types to generate with ReportGenerator |
| `TestingPlatformIgnoreExitCodes` | `8` | Exit codes to ignore as failures when running tests with Microsoft.Testing.Platform |
| `Using` | `System.Globalization` and `System.Text` | Additional global using statements |

### Targets

The following custom targets are provided:

| **Target** | **Description** |
|------------|-----------------|
| `AddGitMetadaAssemblyAttributes` | Embeds additional `[AssemblyMetdata]` into assemblies with Git information |
| `CheckCodeCoverageThreshold` | Checks the code coverage of a project meets a specified line and/or branch threshold when using Microsoft.Testing.Platform |
| `GenerateCoverageReports` | Generates code coverage reports using ReportGenerator |
| `SetGitHubContainerOutputs` | Writes a published container's digest, image and tag to `GITHUB_OUTPUT` |
| `SetNuGetPackageOutputs` | Writes the comma-separated names and version of any published NuGet packages to `GITHUB_OUTPUT` |

## Building and Testing

Compiling the application yourself requires Git and the [.NET SDK][dotnet-sdk] to be installed.

To build and test the package locally from a terminal/command-line, run the
following set of commands:

```powershell
git clone https://github.com/martincostello/build-kit.git
cd build-kit
./build.ps1
```

## Feedback

Any feedback or issues can be added to the issues for this project in [GitHub][issues].

## Repository

The repository is hosted in [GitHub][repo]: <https://github.com/martincostello/build-kit.git>

## License

This project is licensed under the [Apache 2.0][license] license.

[artifacts-output]: https://learn.microsoft.com/dotnet/core/sdk/artifacts-output "Artifacts output layout"
[build-badge]: https://github.com/martincostello/build-kit/actions/workflows/build.yml/badge.svg?branch=main&event=push
[build-status]: https://github.com/martincostello/build-kit/actions?query=workflow%3Abuild+branch%3Amain+event%3Apush "Continuous Integration for this project"
[coverage-badge]: https://codecov.io/gh/martincostello/build-kit/branch/main/graph/badge.svg
[coverage-report]: https://codecov.io/gh/martincostello/build-kit "Code coverage report for this project"
[coverlet]: https://github.com/coverlet-coverage/coverlet "Coverlet on GitHub"
[dotnet-sdk]: https://dotnet.microsoft.com/download "Download the .NET SDK"
[issues]: https://github.com/martincostello/build-kit "Issues for this project on GitHub.com"
[license]: https://www.apache.org/licenses/LICENSE-2.0.txt "The Apache 2.0 license"
[msbuild-escape]: https://learn.microsoft.com/visualstudio/msbuild/how-to-escape-special-characters-in-msbuild "Escape special characters in MSBuild"
[mtp]: https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro "Microsoft.Testing.Platform overview"
[nuget-package]: https://www.nuget.org/packages/MartinCostello.BuildKit "MartinCostello.BuildKit on NuGet.org"
[package-badge-downloads]: https://img.shields.io/nuget/dt/MartinCostello.BuildKit?logo=nuget&label=Downloads&color=blue
[package-badge-version]: https://img.shields.io/nuget/v/MartinCostello.BuildKit?logo=nuget&label=Latest&color=blue
[package-download]: https://www.nuget.org/packages/MartinCostello.BuildKit "Download MartinCostello.BuildKit from NuGet"
[repo]: https://github.com/martincostello/build-kit "This project on GitHub.com"
[reportgenerator]: https://github.com/danielpalme/ReportGenerator "ReportGenerator on GitHub"
[scorecard-badge]: https://api.securityscorecards.dev/projects/github.com/martincostello/build-kit/badge
[scorecard-report]: https://securityscorecards.dev/viewer/?uri=github.com/martincostello/build-kit "OpenSSF Scorecard for this project"
[source]: https://github.com/martincostello/build-kit/tree/main/src/BuildKit "Source code for this project"
