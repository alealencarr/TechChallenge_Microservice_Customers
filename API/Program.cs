using API;
using API.Extensions;
using Infrastructure;
using Serilog;

Log.Logger = LogExtensions.ConfigureLog();

try
{
    Log.Information("Iniciando aplica��o...");

    var builder = WebApplication.CreateBuilder(args);


    builder.Services
           .AddPresentation(builder.Configuration)
           .AddInfrastructure(builder.Configuration)
           .AddHealthChecks().AddHealthApi().AddHealthDb(builder.Configuration);

    var app = builder.Build();

    await app.InitializeApp(Log.Logger);

    app.RegisterPipeline();
    app.AddHealthChecks();
    app.MapGet("/", () => Results.Ok("TechChallenge API - Running"));
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplica��o terminou inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
