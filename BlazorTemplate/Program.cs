using MudBlazor.Services;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureLogger();

Logger logger = LogManager.GetCurrentClassLogger();
logger.Info("Start");

try
{

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    DataContextOptions dataContextOptions = new();
    builder.Configuration.GetSection("DataContext").Bind(dataContextOptions);

    builder.Services.ConfigureDataAccess(dataContextOptions)
                    .ConfigureApplication()
                    .ConfigureJwt(builder.Configuration)
                    .ConfigureMail(builder.Configuration)
                    .AddConfiguration(builder.Configuration)
                    .AddGravatar()
                    .AddValidators()
                    .AddAuthenticate()
                    .AddServices();

    builder.Services.AddMudServices();
    builder.Services.AddHealthChecks();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    app.MapHealthChecks("/health");

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.UseRequestLocalization(new RequestLocalizationOptions()
        .AddSupportedCultures(new[] { "fr-FR", "en-EN" })
        .AddSupportedUICultures(new[] { "en-EN", "en-EN" }));

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}