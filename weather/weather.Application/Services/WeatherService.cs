namespace weather.Application.Services;

using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Json;
using weather.Application.Contracts;
using weather.Application.Interfaces;

public sealed class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    private readonly ConcurrentDictionary<long, (WeatherResponse Response, DateTime ExpireAtUtc)> _cache = new();

    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherResponse> GetWeatherByCityIdAsync(long cityId, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(cityId, out var cached) && cached.ExpireAtUtc > DateTime.UtcNow)
        {
            return cached.Response;
        }

        // Placeholder: call a hypothetical external API endpoint. Replace with actual provider.
        // Example URL: /api/external/weather?cityId=xxx
        var response = await _httpClient.GetFromJsonAsync<WeatherResponse>($"weather/external?cityId={cityId}", cancellationToken)
                        ?? new WeatherResponse(cityId, $"City-{cityId}", "Sunny", 26.5m, 55m, 10m, DateTime.UtcNow);

        response = response with { ObservedAtUtc = DateTime.UtcNow };
        _cache[cityId] = (response, DateTime.UtcNow.Add(CacheTtl));
        return response;
    }
}