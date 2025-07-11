// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.BuildKit;

public class VersionTests(ITestOutputHelper outputHelper)
    : IntegrationTests(outputHelper)
{
    private static readonly string[] PropertyNames =
    [
        "FileVersion",
        "VersionPrefix",
        "VersionSuffix",
    ];

    [Fact]
    public async Task StabilizeVersion_Does_Not_Set_VersionSuffix()
    {
        // Arrange
        var properties = new Dictionary<string, string?>()
        {
            ["GITHUB_ACTIONS"] = "true",
            ["StabilizeVersion"] = "true",
        };

        // Act
        var actual = await EvaluateProperties(
            PropertyNames,
            properties,
            TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldContainKey("FileVersion");
        actual.ShouldContainKey("VersionPrefix");
        actual.ShouldContainKeyAndValue("VersionSuffix", string.Empty);
        actual["FileVersion"].ShouldNotBeNullOrWhiteSpace();
        actual["VersionPrefix"].ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task StabilizeVersion_Does_Not_Set_VersionSuffix_In_GitHub_Actions()
    {
        // Arrange
        var properties = new Dictionary<string, string?>()
        {
            ["GITHUB_ACTIONS"] = "true",
            ["GITHUB_BASE_REF"] = string.Empty,
            ["GITHUB_HEAD_REF"] = string.Empty,
            ["GITHUB_REF"] = "refs/heads/main",
            ["GITHUB_REF_NAME"] = "main",
            ["GITHUB_SHA"] = "1234567890abcdef1234567890abcdef12345678",
            ["GITHUB_REPOSITORY"] = "martincostello/buildkit",
            ["GITHUB_RUN_NUMBER"] = "4",
            ["StabilizeVersion"] = "true",
            ["VersionPrefix"] = "1.2.3",
        };

        // Act
        var actual = await EvaluateProperties(
            PropertyNames,
            properties,
            TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldContainKeyAndValue("FileVersion", "1.2.3.4");
        actual.ShouldContainKeyAndValue("VersionPrefix", "1.2.3");
        actual.ShouldContainKeyAndValue("VersionSuffix", string.Empty);
    }
}
