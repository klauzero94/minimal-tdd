using System.Text;
using API.Routes;
using Kernel.API;
using Kernel;
using IoC;
using App.Mapper;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = EnvName.Development
});

string apiName = "Minimal TDD";
string apiVersion = "v1";

// Read environment var from Heroku or Local
string? appSettings = Environment.GetEnvironmentVariable($"appsettings.{Environment.GetEnvironmentVariable("environment")}") ??
    File.ReadAllText($"appsettings.{builder.Environment.EnvironmentName}.json");
builder.Configuration.AddConfiguration(new ConfigurationBuilder()
    .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(appSettings)))
    .Build());

builder.Services.AddEndpointsApiExplorer();

Mapper.Add(builder.Services);
builder.Services.AddCors();
IoCRepository.Configure(builder.Services);
IoCService.Configure(builder.Services);
ConfigureKernel.Configure(builder.Services);

builder.Services.AddScoped<ProductRoutes>();

Swagger.AddSwagger(builder.Services, apiName, apiVersion);

var app = builder.Build();
var env = Environment.GetEnvironmentVariable("environment") ?? builder.Environment.EnvironmentName;

Swagger.SetSwagger(app, env, apiName);
app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyMethod()
    .WithExposedHeaders(HeadersProp.XTotalCount)
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());
ConfigureKernel.UseMiddlewares(app, env);

using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
services.GetRequiredService<ProductRoutes>().MapActions(app);

await app.RunAsync();