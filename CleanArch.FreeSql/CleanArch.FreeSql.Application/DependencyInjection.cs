namespace CleanArch.FreeSql.Application;

using CleanArch.FreeSql.Application.Interfaces;
using CleanArch.FreeSql.Application.Services;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        return services;
    }
}