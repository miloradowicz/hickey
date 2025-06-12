namespace app.Models;

internal record class DeviceOperationResult(
  DeviceOperationResultCode Result,
  DeviceBase? Device = null
);
