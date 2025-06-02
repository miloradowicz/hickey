namespace shared.Models;

public record class DeviceReport(
  string Name,
  DeviceStatus Status,
  ulong? Uptime
);