# Coding Agent Instructions

This file provides guidance to coding agents when working with code in this repository.

## Build, test, and lint commands

Use `pwsh .\build.ps1` for the normal local validation flow. The script bootstraps the exact SDK from `global.json` into `.dotnet` if needed, packs `src\BuildKit\MartinCostello.BuildKit.csproj`, and then runs the test suite in `Release`.

Use `pwsh .\build.ps1 -SkipTests` when you only need to verify packaging or inspect pack output.

Use `dotnet test --configuration Release` to run the full test suite directly.

Use `dotnet test .\tests\BuildKit.Tests\MartinCostello.BuildKit.Tests.csproj --filter "FullyQualifiedName~VersionTests"` to run one test class.

Use `dotnet test .\tests\BuildKit.Tests\MartinCostello.BuildKit.Tests.csproj --filter "FullyQualifiedName=MartinCostello.BuildKit.SelfTests.Tests_Are_Strong_Named"` to run a single test.

If you change the container sample or container-related targets, the CI build also exercises `dotnet publish .\samples\ContainerApp --arch x64 --os linux -p:PublishProfile=DefaultContainer -p:SetGitHubContainerPublishingOutputs=true`.

There is no single repo script for all linting, but CI linting in `.github\workflows\lint.yml` enforces PowerShell analysis, Markdown linting, `actionlint`, and `zizmor`. The local PowerShell lint command used in CI is:

```powershell
$settings = @{
    IncludeDefaultRules = $true
    Severity = @("Error", "Warning")
}
$issues = Invoke-ScriptAnalyzer -Path $PWD -Recurse -ReportSummary -Settings $settings
if ($issues.Count -gt 0) { exit 1 }
```

## High-level architecture

This repository builds a single NuGet package, `MartinCostello.BuildKit`, that is mostly a bundle of reusable MSBuild `.props` and `.targets` files plus supporting assets such as rulesets, runsettings, strong-name keys, xUnit config, and a localhost certificate. The package project itself is `src\BuildKit\MartinCostello.BuildKit.csproj`; most of the actual behavior lives under `src\BuildKit\build\` and `src\BuildKit\buildMultiTargeting\`.

Inside this repo, the package is dogfooded through the root `Directory.Build.props` and `Directory.Build.targets`, which import `src\BuildKit\build\MartinCostello.BuildKit.props` and `.targets` into every project in the solution. If you change package behavior, remember that the repo itself is consuming those imports during normal builds.

The entrypoint props/targets files are composition layers. `build\MartinCostello.BuildKit.props` imports modular files for GitHub Actions metadata, versioning, build defaults, analyzers, containers, packages, Sentry, and testing. `build\MartinCostello.BuildKit.targets` then imports the matching target modules for build, containers, web, packages, and test behavior. Most feature work belongs in one of those focused files rather than in the entrypoint wrappers.

The `samples\` projects are intentionally small consumers of the package and are part of the architecture, not just examples. `ConsoleApp` validates defaults like strong naming and `InternalsVisibleTo` support for test doubles. `ContainerApp` exercises container publishing plus the `RestoreNpmPackages` flow through a custom `BundleAssets` target that runs `npm test`. `NuGetPackage` exercises packing and package metadata behavior.

The tests are largely integration tests for MSBuild behavior rather than unit tests of helper code. `tests\BuildKit.Tests\IntegrationTests.cs` creates a temporary SDK-style project, imports the package props/targets directly, and uses `dotnet msbuild -getProperty:` to assert evaluated property values. Other tests assert the observable effects on the sample projects, such as strong naming, package metadata, certificate wiring, and GitHub Actions version computation.

GitHub Actions is part of the behavioral model. Several props/targets read `GITHUB_*` variables to derive branch, PR, tag, repository metadata, package release notes, container outputs, and version suffixes. When changing any GitHub-related logic, update tests that simulate those environment values instead of only testing local defaults.

## Key conventions

The repo expects centralized package management through `Directory.Packages.props`, and repo-wide build behavior comes from the root `Directory.Build.*` imports. Prefer changing shared behavior in the package files instead of per-project overrides unless a sample is intentionally testing an override.

Artifacts output is assumed everywhere. `Directory.Build.props` enables `UseArtifactsOutput`, and test/coverage/package outputs are expected under `artifacts\`. Avoid introducing commands or assertions that rely on legacy `bin\`/`obj\` output locations unless you are working inside a project-level intermediate directory on purpose.

Strong naming is a default, not a special case. The package injects the built-in `.snk` file and exposes `StrongNamePublicKey` and `DynamicProxyGenAssembly2StrongNamePublicKey`, and the samples/tests explicitly verify that assemblies are signed and that internal types can be mocked. If you touch signing or `InternalsVisibleTo` behavior, update both the package logic and the corresponding sample-backed tests.

Test projects are expected to pick up BuildKit testing defaults automatically. `Testing.props` disables CLS compliance for tests, supplies the default runsettings file, and sets up coverage defaults. `Testing.targets` adds implicit `Using` items for `Shouldly` and xUnit based on package references, generates coverage reports, and enforces coverage thresholds from the test project. Follow those conventions instead of re-declaring the same configuration in each test project.

Versioning and package metadata are intentionally GitHub-centric. `Version.props`, `GitHubActions.props`, `Metadata.props`, and `Packages.props` cooperate to derive `VersionPrefix`, `VersionSuffix`, `FileVersion`, repository URLs, release notes, and package outputs from the GitHub Actions environment. Use tests like `VersionTests` when changing that behavior, and preserve the tagged-release and PR-specific branches of the logic.

The container sample is wired to prove more than ASP.NET startup. Its project runs `RestoreNpmPackages` before `npm test`, and the container targets also handle GitHub output emission and optional Pyroscope/Sentry-related behavior. Changes in container-oriented targets should be validated against `samples\ContainerApp` and `ContainerAppTests`, not only by reading the target files in isolation.

The preferred validation loop for this repo is package-first: update a modular props/targets file, then verify the effect through the sample projects and the integration tests. If behavior depends on evaluated MSBuild properties, add or update an `IntegrationTests`-based assertion rather than only checking static file contents.

## General guidelines

- Always ensure code compiles with no warnings or errors and tests pass locally before pushing changes.
- Do not use APIs marked with `[Obsolete]`.
- Bug fixes should **always** include a test that would fail without the corresponding fix.
- Do not introduce new dependencies unless specifically requested.
- Do not update existing dependencies unless specifically requested.
