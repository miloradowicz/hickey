namespace api.Models;

internal class Credentials
{
  public required uint Id { get; init; }
  public required string Username { get; init; }
  public required string Password { get; init; }
}