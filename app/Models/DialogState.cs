namespace app.Models;

internal class DialogState
{
  public string State { get; set; } = string.Empty;
  public required long UserId { get; set; }
}