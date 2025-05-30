namespace shared.Models;

public record struct DeviceReport(
  string? Name,
  DeviceStatus Status,
  DateTime? LastReboot,
  ulong? Uptime
);
