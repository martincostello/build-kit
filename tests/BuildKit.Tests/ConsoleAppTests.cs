﻿// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using MartinCostello.BuildKit.ConsoleApp;
using NSubstitute;

namespace MartinCostello.BuildKit;

public static class ConsoleAppTests
{
    [Fact]
    public static void Console_Application_Does_Not_Throw()
    {
        // Act and Assert
        Should.NotThrow(MartinCostello.BuildKit.ConsoleApp.Program.Main);
    }

    [Fact]
    public static void Application_Is_Strong_Named()
    {
        // Arrange
        var assembly = typeof(MartinCostello.BuildKit.ConsoleApp.Program).Assembly;

        // Act
        var name = assembly.GetName();
        var actual = name.GetPublicKey();

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldNotBeEmpty();
        Convert.ToHexStringLower(actual).ShouldBe("00240000048000009400000006020000002400005253413100040000010001004b0b2efbada897147aa03d2076278890aefe2f8023562336d206ec8a719b06e89461c31b43abec615918d509158629f93385930c030494509e418bf396d69ce7dbe0b5b2db1a81543ab42777cb98210677fed69dbeb3237492a7ad69e87a1911ed20eb2d7c300238dc6f6403e3d04a1351c5cb369de4e022b18fbec70f7d21ed");
    }

    [Fact]
    public static void Internal_Types_Can_Be_Mocked()
    {
        // Arrange
        var target = Substitute.For<HelloGreeter>();
        target.Greet(Arg.Any<string>()).Returns("Salut, BuildKit!");

        // Act
        var actual = target.Greet("Whatever");

        // Assert
        actual.ShouldBe("Salut, BuildKit!");
    }
}
