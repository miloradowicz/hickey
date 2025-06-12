namespace app.Models;

internal class Configuration
{
  public uint Id { get; init; }
  public TimeSpan RebootTime { get; init; }
  public TimeSpan UptimeThreshold { get; init; }
}
