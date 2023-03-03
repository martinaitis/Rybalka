using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Rybalka.Core.AutoMapper;
using Rybalka.Core.Interfaces.Clients;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Core.Interfaces.Services;
using Rybalka.Core.Services;
using Rybalka.Infrastructure.AutoMapper;
using Rybalka.Infrastructure.Clients.WeatherForecast;
using Rybalka.Infrastructure.Data;
using Rybalka.Infrastructure.Repositories;
using RybalkaWebAPI.Attributes.Action;
using RybalkaWebAPI.Configs;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var serilogOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .WriteTo.File(
        path: "../logs/webapi-.log", 
        rollingInterval: RollingInterval.Day,
        outputTemplate: serilogOutputTemplate)
    .WriteTo.Console(
        outputTemplate: serilogOutputTemplate)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddAutoMapper(typeof(FishingNoteProfile), typeof(WeatherForecastProfile));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RybalkaApiConnection"));
});

const string CORS_POLICY_ALL_ORIGINS = "AllOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_POLICY_ALL_ORIGINS,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

builder.Services.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>();

builder.Services.AddScoped<LogAttribute>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IFishingNoteService, FishingNoteService>();
builder.Services.AddScoped<IFishingNoteRepository, FishingNoteRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;
}).AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new StringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "RybalkaWebAPI.xml");
    c.IncludeXmlComments(filePath);
});
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});
builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

app.UseSwagger(options =>
{
    options.PreSerializeFilters.Add((swagger, req) =>
    {
        swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://{req.Host}" } };
    });
});

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerUI(options =>
{
    foreach (var desc in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"../swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
        options.DefaultModelsExpandDepth(-1);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    }
});

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseCors(CORS_POLICY_ALL_ORIGINS);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
