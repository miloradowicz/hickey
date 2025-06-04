using shared.Models;

namespace api.Models;

internal class Device : DeviceBase
{
  public required uint Id { get; init; }
  public required Credentials Credentials { get; init; }
}
