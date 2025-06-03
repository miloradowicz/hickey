using bot.Models;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

var botConfig = builder.Configuration.GetRequiredSection("Bot").Get<BotConfiguration>()!;
var apiConfig = builder.Configuration.GetRequiredSection("Api").Get<ApiConfiguration>()!;

builder.Services.AddHttpClient("tg-bot").AddTypedClient(httpClient => new TelegramBotClient(botConfig.Token, httpClient));
builder.Services.AddHttpClient("api-client").ConfigureHttpClient((httpClient) =>
{
  httpClient.BaseAddress = apiConfig.Url;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// app.Urls.Add("http://localhost:80");
// app.Urls.Add("http://localhost:88");
// app.Urls.Add("https://localhost:443");
// app.Urls.Add("https://localhost:8443");
app.UseHttpsRedirection();

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