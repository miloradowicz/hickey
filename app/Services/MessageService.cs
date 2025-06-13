using System.Text.RegularExpressions;
using app.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace app.Services;

internal class StateService(HickeyContext context, IHttpClientFactory clientFactory, TelegramBotClient telegramClient) : IMessageService
{
  private readonly HickeyContext context = context;
  private readonly TelegramBotClient telegramClient = telegramClient;

  public void Transition(Update update)
  {

  }
}