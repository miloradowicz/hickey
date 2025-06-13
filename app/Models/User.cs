using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace app.Models;

internal class User
{
  [SetsRequiredMembers]
  public User(string name, long telegramId)
  {
    (this.Name, this.TelegramId) = (name, telegramId);
  }

  [SetsRequiredMembers]
  public User(User user) : this(user.Name, user.TelegramId) { }

  public uint Id { get; set; }
  public required string Name { get; set; }
  public required long TelegramId { get; set; }

  public void Deconstruct(out string Name, out long TelegramId, out string TelegramUsername)
  {
    throw new NotImplementedException();
  }

  public void Deconstruct(out string Name, out long TelegramId)
  {
    (Name, TelegramId) = (this.Name, this.TelegramId);
  }
}