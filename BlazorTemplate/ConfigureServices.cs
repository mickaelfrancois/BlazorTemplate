using BlazorTemplate.Services.Authentication;
using BlazorTemplate.Services.Layout;
using BlazorTemplate.Services.Preferences;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MySqlConnector.Logging;
using NLog.Web;
using System.Reflection;

namespace BlazorTemplate;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureLogger(this IServiceCollection services)
    {
        NLog.LogManager.Setup().LoadConfigurationFromAppSettings();

        MySqlConnectorLogManager.Provider = new NLogLoggerProvider();

        return services;
    }


    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }


    public static IServiceCollection AddAuthenticate(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        services.AddScoped<IdentityStorage>();
        services.AddScoped<CustomAuthenticationStateProviders>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProviders>());

        return services;
    }


    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<UserPreferencesService>();
        services.AddScoped<LayoutService>();

        return services;
    }


    public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<IdentityConfiguration>(configuration.GetSection("Identity"));

        return services;
    }
}
