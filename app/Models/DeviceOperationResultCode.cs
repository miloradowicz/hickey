namespace app.Models;

internal enum DeviceOperationResultCode
{
  DeviceAdded,
  DeviceUpdated,
  DeviceRemoved,
  DeviceAlreadyExists,
  DeviceNotFound,
  StatusReceived,
  RebootRequested,
  BadResponse,
  BadConfiguration,
  AccessDenied,
  NoResponse,
  OperationFailed,
}