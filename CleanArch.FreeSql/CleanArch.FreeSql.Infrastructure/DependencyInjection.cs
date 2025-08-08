namespace CleanArch.FreeSql.Infrastructure;

using CleanArch.FreeSql.Domain.Abstractions.Repositories;
using CleanArch.FreeSql.Infrastructure.Persistence;
using CleanArch.FreeSql.Infrastructure.Repositories;
using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IFreeSql>(sp => FreeSqlFactory.CreateFreeSql(configuration));
        services.AddScoped<ITodoRepository, TodoRepository>();
        return services;
    }
}