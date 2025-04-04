﻿// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.BuildKit.ConsoleApp;

internal static class Program
{
    public static void Main()
    {
        var greeter = new HelloGreeter();
        var greeting = greeter.Greet("World");

        Console.WriteLine(greeting);
    }
}
