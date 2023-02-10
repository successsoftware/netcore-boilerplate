using CoreApiTemplate.Application;
using CoreApiTemplate.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSS.AspNetCore.Extensions.Handlers;
using SSS.AspNetCore.Extensions.Serilog;
using SSS.AspNetCore.Extensions.Swagger;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.AddSerilogFromConfiguration();

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddApplication();

// Add configuration for Proxy/Refit Client
builder.Services.AddProxies(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI();
}

using var scope = app.Services.CreateScope();

var seeder = scope.ServiceProvider.GetService<SeedData>();

if (seeder is null) throw new ArgumentNullException(nameof(seeder), "{seedData} is not null");

seeder.PopulateData();

app.UseGlobalExceptionHandler();

app.UseAuthentication();

app.MapControllers();

// How to use logging in Program.cs file		
app.Logger.LogInformation("The application started");

app.Run();

// Use to add reference in Test project
public partial class Program { }