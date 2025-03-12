// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.BuildKit.ConsoleApp;

internal class HelloGreeter : IGreeter
{
    public virtual string Greet(string name) => $"Hello, {name}!";
}
