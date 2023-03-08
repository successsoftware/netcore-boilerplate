using System.Text.Json;

using CleanArchitecture.Net7.WebApi.Configurations;
using CleanArchitecture.Net7.WebApi.Data;
using CleanArchitecture.Net7.WebApi.Extensions;

using CleanArchitecuture.Net7.EmailProvider;

using FastEndpoints.Swagger;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

logger.Information("Starting up");

try
{

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
        options.AllowSynchronousIO = false;
    });

    if (builder.Environment.IsProduction())
    {
        builder.Logging.ClearProviders();
    }
    builder.Services.AddSingleton(logger);

    builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);

    // Start custom configuration
    // builder.Services.AddSqlDbContext<AppIdentityDbContext>(builder.Configuration); // Uncomment to use SQL
    builder.Services.AddNpgsqlDbContext<AppIdentityDbContext>(builder.Configuration); // PostgreSQL
    if (!builder.Environment.IsEnvironment("Testing"))
    {
        builder.Services.AddAuth(builder.Configuration);
    }
    builder.Services.AddEmailProvider(builder.Configuration);
    // End custom configuration

    builder.Services.AddFastEndpoints(options =>
    {
        options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
    });

    builder.Services.AddSwaggerDoc();

    builder.Services.AddTransient<DbSeeder>();
    builder.Services.RegisterServiceLayerDi();

    var app = builder.Build();

    var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();
    await dbContext.Database.MigrateAsync();
    var dbSeeder = app.Services.CreateScope().ServiceProvider.GetService<DbSeeder>();
    await dbSeeder.SeedRoleASync();

    app.UseDefaultExceptionHandler();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseFastEndpoints(options =>
    {
        options.Versioning.Prefix = "v";
        options.Endpoints.RoutePrefix = "api";
        options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Endpoints.Configurator = ep =>
        {
            ep.Description(b => b
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .ProducesProblemFE(StatusCodes.Status401Unauthorized)
                .ProducesProblemFE(StatusCodes.Status500InternalServerError));
        };
        options.Errors.ResponseBuilder = (failures, ctx, statusCode) => statusCode switch
            {
                StatusCodes.Status400BadRequest => new ValidationProblemDetails(failures.GroupBy(f => f.PropertyName.ToCamelCase()).ToArray().ToDictionary(e =>
                        e.Key, e => e.Select(m => m.ErrorMessage).ToArray()))
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = statusCode,
                    Instance = ctx.Request.Path,
                    Extensions = { { "traceId", ctx.TraceIdentifier } }
                },
                _ => new ValidationProblemDetails
                {
                    Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
                    Title = "Internal Server Error",
                    Status = statusCode,
                    Instance = ctx.Request.Path,
                    Extensions = { { "traceId", ctx.TraceIdentifier } }
                }
            };
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi3(x => x.ConfigureDefaults());
    }

    app.Run();

}
catch (Exception ex)
{
    logger.Fatal(ex, "Unhandled exception");
}
finally
{
    logger.Error("Shut down complete");
    Log.CloseAndFlush();
}

public partial class Program { }