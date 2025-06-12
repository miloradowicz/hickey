
using System.Diagnostics.CodeAnalysis;

namespace app.Models;

internal class Credentials
{
  [SetsRequiredMembers]
  public Credentials(string username, string password)
  {
    (this.Username, this.Password) = (username, password);
  }

  [SetsRequiredMembers]
  public Credentials(Credentials credentials)
  {
    (this.Username, this.Password) = credentials;
  }

  public uint Id { get; set; }
  public required string Username { get; set; }
  public required string Password { get; set; }

  public void Deconstruct(out string Username, out string Password)
  {
    (Username, Password) = (this.Username, this.Password);
  }
}