using bot.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.GetRequiredSection("Bot").Get<BotConfiguration>()!;

builder.Services.AddHttpClient("tgwebhook").AddTypedClient(httpClient => new TelegramBotClient(configuration.Token, httpClient));

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

app.UseHttpsRedirection();

app.MapGet("/set-hook", OnSetHook);

app.MapPost("/on-update", OnUpdate);

app.Run();

async Task<string> OnSetHook(ITelegramBotClient bot)
{
  await bot.SetWebhook(configuration.WebHookUrl);

  return $"Webhook set to {configuration.WebHookUrl}";
}

async void OnUpdate(ITelegramBotClient bot, Update update)
{
  if (update.Message is null) return;
  if (update.Message.Text is null) return;

  var msg = update.Message;

  Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");

  await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
}