// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace MartinCostello.BuildKit;

public class ContainerAppTests
{
    [Fact]
    public async Task Can_Get_Weather_Forecast()
    {
        // Arrange
        var context = TestContext.Current;
        var outputHelper = context.TestOutputHelper!;

        await using var fixture = new WebApplicationFactory<WeatherForecast>()
            .WithWebHostBuilder((p) => p.ConfigureLogging((r) => r.AddXUnit(outputHelper)));

        using var client = fixture.CreateClient();

        // Act
        using var response = await client.GetAsync("/weatherforecast", context.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<WeatherForecast[]>(context.CancellationToken);

        actual.ShouldNotBeNull();
        actual.ShouldAllBe((p) => p != null);
        actual.Length.ShouldBe(5);
    }
}
