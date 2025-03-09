// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.BuildKit;

public static class ConsoleAppTests
{
    [Fact]
    public static void Console_Application_Does_Not_Throw()
    {
        // Act and Assert
        Should.NotThrow(MartinCostello.BuildKit.ConsoleApp.Program.Main);
    }
}
