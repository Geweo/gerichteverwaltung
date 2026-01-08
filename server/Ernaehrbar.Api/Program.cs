using Ernaehrbar.Adapters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ernährbär API",
        Version = "v1",
        Description = "API für den Rezept- & Zutatenplaner mit Bring-Anbindung"
    });
});

// Add Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ernährbär API v1");
        options.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}

// Always enable Swagger JSON endpoint for API client generation
app.UseSwagger();

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

// Log startup information
Log.Information("🚀 Ernährbär API starting...");
var swaggerUrl = app.Environment.IsDevelopment() 
    ? $"http://localhost:5000/swagger" 
    : "N/A";
Log.Information("📚 Swagger UI available at: {SwaggerUrl}", swaggerUrl);

app.Run();

