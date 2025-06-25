using DataAccess.Database;
using E_Commerce_2025.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Service.Implementations.CategoryRepositories;
using Service.Implementations.ProductRepositories;
using Service.Interfaces.CategoryInterfaces;
using Service.Interfaces.ProductInterfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("https://localhost:7042")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

DotNetEnv.Env.Load();
var connection = Environment.GetEnvironmentVariable("connection");
var key = Environment.GetEnvironmentVariable("Key");
if (string.IsNullOrEmpty(key))
    throw new Exception("JWT secret key is not set in the environment variables.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection)
);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<ICategory, CategoryRepo>();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<InventoryHub>("/hubs/inventory");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
