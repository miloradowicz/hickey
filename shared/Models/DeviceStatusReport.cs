namespace shared.Models;

public record class DeviceStatusReport(
  DeviceStatus Status,
  ulong? Uptime
);