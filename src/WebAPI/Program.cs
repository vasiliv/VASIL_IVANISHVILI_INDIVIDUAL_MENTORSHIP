using BL;
using BL.Commands;
using DAL.Context;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection;
using WebAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//My addings
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WeatherForecast>();
builder.Services.AddMediatR(typeof(CurrentWeatherCommand).Assembly);

builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
