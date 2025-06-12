namespace app.Models;

internal class BotConfiguration
{
  public required string Token { get; init; }
  public string? WebHookUrl { get; init; }
}
