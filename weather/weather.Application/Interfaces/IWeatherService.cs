namespace weather.Application.Interfaces;

using weather.Application.Contracts;

public interface IWeatherService
{
    Task<WeatherResponse> GetWeatherByCityIdAsync(long cityId, CancellationToken cancellationToken = default);
}