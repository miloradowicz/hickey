namespace shared.Models;

public class DeviceBase
{
  public required string Name { get; init; }
  public required string Address { get; init; }
  public required ushort Port { get; init; }
}
