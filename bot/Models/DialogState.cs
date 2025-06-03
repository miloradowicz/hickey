using Telegram.Bot.Types;

namespace bot.Models;

internal class DialogState
{
  public string State { get; set; } = string.Empty;
  public required long UserId { get; set; }
}