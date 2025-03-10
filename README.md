# MartinCostello.BuildKit 🛠️

[![NuGet][package-badge-version]][package-download]
[![NuGet Downloads][package-badge-downloads]][package-download]

[![Build status][build-badge]][build-status]
[![codecov][coverage-badge]][coverage-report]

[![OpenSSF Scorecard][scorecard-badge]][scorecard-report]

## Introduction

A NuGet package containing reusable MSBuild targets for my other .NET projects.

### Installation

To install the library from [NuGet](https://www.nuget.org/packages/MartinCostello.BuildKit/ "MartinCostello.BuildKit on NuGet.org") using the .NET SDK run:

```sh
dotnet add package MartinCostello.BuildKit
```

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

[build-badge]: https://github.com/martincostello/build-kit/actions/workflows/build.yml/badge.svg?branch=main&event=push
[build-status]: https://github.com/martincostello/build-kit/actions?query=workflow%3Abuild+branch%3Amain+event%3Apush "Continuous Integration for this project"
[coverage-badge]: https://codecov.io/gh/martincostello/build-kit/branch/main/graph/badge.svg
[coverage-report]: https://codecov.io/gh/martincostello/build-kit "Code coverage report for this project"
[dotnet-sdk]: https://dotnet.microsoft.com/download "Download the .NET SDK"
[issues]: https://github.com/martincostello/build-kit "Issues for this project on GitHub.com"
[license]: https://www.apache.org/licenses/LICENSE-2.0.txt "The Apache 2.0 license"
[package-badge-downloads]: https://img.shields.io/nuget/dt/MartinCostello.BuildKit?logo=nuget&label=Downloads&color=blue
[package-badge-version]: https://img.shields.io/nuget/v/MartinCostello.BuildKit?logo=nuget&label=Latest&color=blue
[package-download]: https://www.nuget.org/packages/MartinCostello.BuildKit "Download MartinCostello.BuildKit from NuGet"
[repo]: https://github.com/martincostello/build-kit "This project on GitHub.com"
[scorecard-badge]: https://api.securityscorecards.dev/projects/github.com/martincostello/build-kit/badge
[scorecard-report]: https://securityscorecards.dev/viewer/?uri=github.com/martincostello/build-kit "OpenSSF Scorecard for this project"
