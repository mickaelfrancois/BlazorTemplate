using Blazor.Infrastructure.Gravatar;
using Blazor.Infrastructure.JWT;
using Blazor.Infrastructure.Mail;
using FluentEmail.MailKitSmtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Blazor.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureMail(this IServiceCollection services, IConfiguration configuration)
    {
        SmtpClientConfiguration smtpClientConfiguration = new();
        configuration.GetSection(SmtpClientConfiguration.Key).Bind(smtpClientConfiguration);

        SmtpClientOptions smtpClientOptions = new();
        configuration.GetSection(SmtpClientConfiguration.Key).Bind(smtpClientOptions);
        services.Configure<SmtpClientOptions>(configuration.GetSection(nameof(SmtpClientOptions)));

        services.AddSingleton(smtpClientOptions);
        services.AddScoped<IMailService, MailService>();

        services.AddFluentEmail(smtpClientConfiguration.DefaultSender)
            .AddRazorRenderer(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplates"))
            .AddMailKitSender(smtpClientOptions);

        return services;
    }


    public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        JwtConfiguration jwtConfiguration = new();
        configuration.GetSection(JwtConfiguration.Key).Bind(jwtConfiguration);
        services.Configure<JwtConfiguration>(configuration.GetSection(JwtConfiguration.Key));

        services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IConfirmEmailTokenGenerator, ConfirmEmailTokenGenerator>();
        services.AddScoped<IForgotPasswordTokenGenerator, ForgotPasswordTokenGenerator>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<TokenValidatorService>();
        services.AddScoped<JwtSecurityTokenHandler>();
        services.AddScoped<IJwtLoginService, JwtLoginService>();

        return services;
    }


    public static IServiceCollection AddGravatar(this IServiceCollection services)
    {
        services.AddSingleton<IGravatarService, GravatarService>();

        return services;
    }

}
