using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using RybalkaWebAPI.Attributes;
using Serilog;
using Serilog.Events;

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "RybalkaWebAPI - V1",
            Version = "v1"
        }
     );

    var filePath = Path.Combine(AppContext.BaseDirectory, "RybalkaWebAPI.xml");
    c.IncludeXmlComments(filePath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseCors(CORS_POLICY_ALL_ORIGINS);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
