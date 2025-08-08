namespace weather.Application.Contracts;

public record WeatherRequest(long CityId);

public record WeatherResponse(
    long CityId,
    string CityName,
    string Condition,
    decimal TemperatureC,
    decimal HumidityPercent,
    decimal WindKph,
    DateTime ObservedAtUtc
);