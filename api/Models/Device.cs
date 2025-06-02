namespace api.Models;

#nullable disable
internal class Device
{
  public uint Id { get; init; }
  public string Name { get; init; }
  public string Address { get; init; }
  public ushort Port { get; init; }
  public string Username { get; init; }
  public string Password { get; init; }
}
