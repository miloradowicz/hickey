using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using shared.Endpoints;

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

app.MapGet($"/{ApiEndpoint.Status}", async (IDeviceService deviceService) =>
{
  return await deviceService.GetAllDeviceStatuses();
});

app.MapGet($"/{ApiEndpoint.Status}/{{id}}", async ([FromRoute] uint id, IDeviceService deviceService) =>
{
  var result = await deviceService.GetDeviceStatus(id);

  return result is not null
    ? Results.Ok(result)
    : Results.NotFound("No device with such id.");
});

app.MapPut($"/{ApiEndpoint.Reboot}", async (IDeviceService deviceService) =>
{
  return await deviceService.RebootAllDevices();
});

app.MapPut($"/{ApiEndpoint.Reboot}/{{id}}", async ([FromRoute] uint id, IDeviceService deviceService) =>
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


/////////////////////////////////////

var botConfig = builder.Configuration.GetRequiredSection("Bot").Get<BotConfiguration>()!;
var apiConfig = builder.Configuration.GetRequiredSection("Api").Get<ApiConfiguration>()!;

builder.Services.AddHttpClient("tg-bot").AddTypedClient(httpClient => new TelegramBotClient(botConfig.Token, httpClient));
builder.Services.AddHttpClient("api-client").ConfigureHttpClient((httpClient) =>
{
  httpClient.BaseAddress = apiConfig.Url;
});



app.MapGet("/set-hook", OnSetHook);
app.MapPost("/on-update", OnUpdate);

app.Run();

async Task<string> OnSetHook([FromServices] ITelegramBotClient bot)
{
  if (botConfig.WebHookUrl is not null)
  {
    await bot.SetWebhook(botConfig.WebHookUrl);

    return $"Webhook has been updated to {botConfig.WebHookUrl}";
  }

  return "Webhook has not been updated.";
}

async void OnUpdate([FromBody] Update update, [FromServices] ITelegramBotClient bot)
{
  if (update.Message is null) return;
  if (update.Message.Text is null) return;

  var msg = update.Message;

  Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");

  await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
}