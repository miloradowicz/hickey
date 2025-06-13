using Microsoft.EntityFrameworkCore;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;
using Npgsql.Replication;

var builder = WebApplication.CreateBuilder(args);

var botConfig = builder.Configuration.GetRequiredSection("Bot").Get<BotConfiguration>()!;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("tg-bot").AddTypedClient(httpClient => new TelegramBotClient(botConfig.Token, httpClient));
builder.Services.AddDbContext<HickeyContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
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

app.MapGet("/set-hook", OnSetHook);
app.MapGet("/on-schedule", OnSchedule);
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

async Task OnSchedule([FromServices] IDeviceService deviceService)
{
  await deviceService.RebootAllDevices();
}

async Task OnUpdate(
  [FromBody] Update update,
  [FromServices] IDeviceService deviceService,
  [FromServices] IUserService userService,
  [FromServices] ITelegramBotClient bot
  )
{
  if (update.Message is null) return;
  if (update.Message.Text is null) return;
  if (update.Message.From is null) return;

  var msg = update.Message;
  var user = msg.From;

  if (userService.FindUserByTelegramId(user.Id) is null)
  {
    if (msg.Text == "/addme")
    {
      await bot.SendMessage(msg.Chat, "Someone will review your request.");

    }
    else
    {
      await bot.SendMessage(msg.Chat, "Unauthorized.");
    }
  }

  var messageRegex = new Regex(@"^(?<command>/status|/reboot)(?<arg>\d+)");
  var match = messageRegex.Match(msg.Text);
  if (match.Success)
  {
    uint arg;
    if (!uint.TryParse(match.Groups["arg"].Value, out arg))
    {
      await bot.SendMessage(msg.Chat, "Invalid argument.");
    }

    switch (match.Groups["command"].Value)
    {
      case "/status":
        if (arg == 0)
        {
          List<Task<DeviceStatusOperationResult>> requests = [];
          var results = await deviceService.GetAllDeviceStatuses();

          await bot.SendMessage(msg.Chat, string.Join('\n', from r in results select $"{r.Device?.Name ?? "unknown device"}: {r.Result}"));
        }
        else
        {
          var result = await deviceService.GetDeviceStatus(arg);

          await bot.SendMessage(msg.Chat, $"{result.Device?.Name ?? "unknown device"} - {result.Result}");
        }
        break;

      case "/reboot":
        if (arg == 0)
        {
          List<Task<DeviceStatusOperationResult>> requests = [];
          var results = await deviceService.RebootAllDevices();

          await bot.SendMessage(msg.Chat, string.Join('\n', from r in results select $"{r.Device?.Name ?? "unknown device"}: {r.Result}"));
        }
        else
        {
          var result = await deviceService.RebootDevice(arg);

          await bot.SendMessage(msg.Chat, $"{result.Device?.Name ?? "unknown device"} - {result.Result}");
        }
        break;
    }
  }
}

/////////////////////////////////////






