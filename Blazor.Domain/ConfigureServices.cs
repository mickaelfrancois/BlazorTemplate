using Microsoft.Extensions.DependencyInjection;
using NLog;
using SqlKata.Compilers;

namespace Blazor.Domain;

public static class ConfigureServices
{
    private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

    public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, DataContextOptions options)
    {
        services.AddScoped<IDbConnection>(c =>
        {
            return new MySqlConnection(options.ConnectionString);
        });

        services.AddScoped<DataAccess>();

        services.AddScoped<Compiler, MySqlCompiler>();

        services.AddTransient<QueryFactory>(c =>
        {
            return new QueryFactory(c.GetService<IDbConnection>(), c.GetService<Compiler>())
            {
                Logger = compiled =>
                {
                    _logger.Debug(compiled.ToString());
                }
            };
        });

        return services;
    }
}
