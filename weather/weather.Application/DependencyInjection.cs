namespace weather.Application;

using weather.Application.Interfaces;
using weather.Application.Services;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        services.AddHttpClient<IWeatherService, WeatherService>(client =>
        {
            client.BaseAddress = new Uri("https://example-weather-provider/");
            client.Timeout = TimeSpan.FromSeconds(5);
        });
        return services;
    }
}