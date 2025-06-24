using DataAccess.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Service.Implementations.CategoryRepositories;
using Service.Implementations.ProductRepositories;
using Service.Interfaces.CategoryInterfaces;
using Service.Interfaces.ProductInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DotNetEnv.Env.Load();

var connection = Environment.GetEnvironmentVariable("connection");

var key = Environment.GetEnvironmentVariable("Key");

if (string.IsNullOrEmpty(key))
{
    throw new Exception("JWT secret key is not set in the environment variables.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connection);
});

builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<ICategory, CategoryRepo>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
