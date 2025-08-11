namespace weather.Infrastructure;

using weather.Domain.Abstractions.Repositories;
using weather.Infrastructure.Persistence;
using weather.Infrastructure.Repositories;
using weather.Infrastructure.Crypto;
using weather.Application.Interfaces;
using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IFreeSql>(sp => FreeSqlFactory.CreateFreeSql(configuration));
        services.AddScoped<ITodoRepository, TodoRepository>();

        services.Configure<CryptoOptions>(configuration.GetSection("Crypto"));
        services.AddSingleton<ICryptoService, CryptoService>();

        return services;
    }
}