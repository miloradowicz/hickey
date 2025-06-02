using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddScoped<IDeviceClientFactory, DeviceClientFactory>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/status", async (IDeviceService deviceService) =>
{
  return await deviceService.GetAllDeviceStatuses();
});

app.MapGet("/status/{id}", async ([FromRoute] uint id, IDeviceService deviceService) =>
{
  var result = await deviceService.GetDeviceStatus(id);

  return result is not null
    ? Results.Ok(result)
    : Results.NotFound("No device with such id.");
});

app.MapPut("/reboot", async (IDeviceService deviceService) =>
{
  return await deviceService.RebootAllDevices();
});

app.MapPut("/reboot/{id}", async ([FromRoute] uint id, IDeviceService deviceService) =>
{
  var result = await deviceService.RebootDevice(id);

  return result is not null
    ? Results.Ok(result)
    : Results.NotFound("No device with such id.");
});

app.MapGet("/weatherforecast", () =>
{
  var forecast = Enumerable.Range(1, 5).Select(index =>
    new WeatherForecast
    (
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    ))
    .ToArray();
  return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
