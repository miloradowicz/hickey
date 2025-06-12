namespace app.Models;

internal enum DeviceOperationResultCode
{
  DeviceAdded,
  DeviceUpdated,
  DeviceRemoved,
  DeviceExists,
  DeviceNotFound,
  StatusReceived,
  RebootRequested,
  BadResponse,
  BadConfiguration,
  AccessDenied,
  NoResponse,
  OperationFailed,
}