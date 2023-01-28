using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RybalkaWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;
});

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

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "RybalkaWebAPI.xml");
    c.IncludeXmlComments(filePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseAuthorization();

app.MapControllers();

app.Run();
