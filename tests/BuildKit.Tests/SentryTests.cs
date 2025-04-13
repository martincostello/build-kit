// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.BuildKit;

public class SentryTests(ITestOutputHelper outputHelper)
    : IntegrationTests(outputHelper)
{
    private static readonly string[] PropertyNames =
    [
        "SentryCreateRelease",
        "SentryOrg",
        "SentryProject",
        "SentrySetCommits",
        "SentryUploadSymbols",
        "UseSentry",
    ];

    [Fact]
    public async Task Sentry_Defaults_Are_Correct()
    {
        // Arrange
        var properties = new Dictionary<string, string?>()
        {
            ["ContainerRegistry"] = string.Empty,
            ["GITHUB_ACTIONS"] = "false",
            ["GITHUB_HEAD_REF"] = string.Empty,
            ["GITHUB_REPOSITORY"] = string.Empty,
            ["GITHUB_REPOSITORY_OWNER"] = string.Empty,
            ["SentryAuthToken"] = string.Empty,
            ["SentryProject"] = string.Empty,
        };

        // Act
        var actual = await EvaluateProperties(
            PropertyNames,
            properties,
            TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldContainKeyAndValue("UseSentry", string.Empty);
        actual.ShouldContainKeyAndValue("SentryCreateRelease", string.Empty);
        actual.ShouldContainKeyAndValue("SentryOrg", string.Empty);
        actual.ShouldContainKeyAndValue("SentryProject", string.Empty);
        actual.ShouldContainKeyAndValue("SentrySetCommits", string.Empty);
        actual.ShouldContainKeyAndValue("SentryUploadSymbols", string.Empty);
    }

    [Fact]
    public async Task Sentry_Defaults_Are_Correct_In_CI_With_Project_And_Token()
    {
        // Arrange
        var properties = new Dictionary<string, string?>()
        {
            ["ContainerRegistry"] = "containers.acr.io",
            ["GITHUB_ACTIONS"] = "true",
            ["GITHUB_HEAD_REF"] = string.Empty,
            ["GITHUB_REPOSITORY"] = "octocat/Hello-World",
            ["GITHUB_REPOSITORY_OWNER"] = "octocat",
            ["SentryAuthToken"] = "not-a-secret",
        };

        // Act
        var actual = await EvaluateProperties(
            PropertyNames,
            properties,
            TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldContainKeyAndValue("UseSentry", "true");
        actual.ShouldContainKeyAndValue("SentryCreateRelease", "true");
        actual.ShouldContainKeyAndValue("SentryOrg", "octocat");
        actual.ShouldContainKeyAndValue("SentryProject", "Hello-World");
        actual.ShouldContainKeyAndValue("SentrySetCommits", "true");
        actual.ShouldContainKeyAndValue("SentryUploadSymbols", "true");
    }
}
