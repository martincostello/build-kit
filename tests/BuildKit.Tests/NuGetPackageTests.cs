// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using MartinCostello.BuildKit.NuGetPackage;

namespace MartinCostello.BuildKit;

public static class NuGetPackageTests
{
    [Fact]
    public static void Package_Metadata_Is_Valid()
    {
        // Act and Assert
        PackageMetadata.BuildId.ShouldNotBeNull();
        PackageMetadata.Branch.ShouldNotBeNull();
        PackageMetadata.Branch.ShouldNotBeEmpty();
        PackageMetadata.Commit.ShouldNotBeNull();
        PackageMetadata.Commit.ShouldNotBeEmpty();
        PackageMetadata.Timestamp.ShouldNotBe(default);
        PackageMetadata.Version.ShouldNotBeNullOrWhiteSpace();
        PackageMetadata.DoesNotExist.ShouldNotBeNull();
        PackageMetadata.DoesNotExist.ShouldBeEmpty();
    }
}
