namespace app.Models;

internal record class DeviceStatusOperationResult(
  DeviceOperationResultCode Result,
  DeviceBase? Device = null,
  DeviceStatusReport? Report = null
) : DeviceOperationResult(Result, Device);
