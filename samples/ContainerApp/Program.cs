// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(Random.Shared);
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

string[] summaries =
[
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching",
];

app.MapGet("/weatherforecast", (Random random, TimeProvider timeProvider) =>
{
    var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

    app.Logger.LogInformation("Generating weather forecast for {Date:o}.", today);

    return Enumerable.Range(1, 5).Select((index) =>
        new WeatherForecast(
            today.AddDays(index),
            random.Next(-20, 55),
            random.GetItems(summaries, 1)[0]))
        .ToArray();
});

app.Run();

/// <summary>
/// Represents a weather forecast.
/// </summary>
/// <param name="Date">The date the forecast is for.</param>
/// <param name="TemperatureC">The temperature in Celsius.</param>
/// <param name="Summary">The summary for the forecast.</param>
public sealed record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
