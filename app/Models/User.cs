using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace app.Models;

internal class User(string name, long telegramId, string? telegramUsername)
{
  [SetsRequiredMembers]
  public User(User user) : this(user.Name, user.TelegramId, user.TelegramUsername) { }

  public uint Id { get; set; }
  public required string Name { get; set; } = name;
  public required long TelegramId { get; set; } = telegramId;
  public string? TelegramUsername { get; set; } = telegramUsername;

  public void Deconstruct(out string Name, out long TelegramId, out string TelegramUsername)
  {
    throw new NotImplementedException();
  }
}