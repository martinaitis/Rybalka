using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using RybalkaWebAPI.Attributes;
using RybalkaWebAPI.Data;
using RybalkaWebAPI.Services.WeatherForecast;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var serilogOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}";
var logger = new LoggerConfiguration()
    .WriteTo.File(path: "../logs/webapi-.log",
    rollingInterval: RollingInterval.Day,
    outputTemplate: serilogOutputTemplate)
    .WriteTo.Console(outputTemplate: serilogOutputTemplate)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
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

builder.Services.AddScoped<LogAttribute>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;
}).AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new StringEnumConverter()));

builder.Services.AddHttpClient<WeatherForecastService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Must to set after AddNewtonsoftJson()
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseCors(CORS_POLICY_ALL_ORIGINS);

app.UseAuthorization();

app.MapControllers();

app.Run();
