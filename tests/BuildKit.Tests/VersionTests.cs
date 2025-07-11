// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.BuildKit;

public class VersionTests(ITestOutputHelper outputHelper)
    : IntegrationTests(outputHelper)
{
    private static readonly string[] PropertyNames =
    [
        "VersionPrefix",
        "VersionSuffix",
    ];

    [Fact]
    public async Task StabilizeVersion_Does_Not_Set_VersionSuffix()
    {
        // Arrange
        var properties = new Dictionary<string, string?>()
        {
            ["StabilizeVersion"] = "true",
        };

        // Act
        var actual = await EvaluateProperties(
            PropertyNames,
            properties,
            TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldContainKey("VersionPrefix");
        actual.ShouldContainKeyAndValue("VersionSuffix", string.Empty);
        actual["VersionPrefix"].ShouldNotBeNullOrWhiteSpace();
    }
}
