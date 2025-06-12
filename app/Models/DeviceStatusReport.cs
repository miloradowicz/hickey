namespace app.Models;

internal record class DeviceStatusReport(
  DeviceStatusCode Status,
  ulong? Uptime = null
);