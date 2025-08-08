namespace weather.Infrastructure.Persistence;

using FreeSql;
using Microsoft.Extensions.Configuration;

public static class FreeSqlFactory
{
    public static IFreeSql CreateFreeSql(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? "Data Source=./app.db;";
        var fsql = new global::FreeSql.FreeSqlBuilder()
            .UseConnectionString(global::FreeSql.DataType.Sqlite, connectionString)
            .UseAutoSyncStructure(true)
            .Build();
        return fsql;
    }
}