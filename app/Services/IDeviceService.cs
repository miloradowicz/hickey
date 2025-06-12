using app.Models;

namespace app.Services;

internal interface IDeviceService
{
  public Task<DeviceStatusOperationResult> GetDeviceStatus(uint id);
  public Task<DeviceOperationResult> RebootDevice(uint id);
  public Task<DeviceStatusOperationResult[]> GetAllDeviceStatuses();
  public Task<DeviceOperationResult[]> RebootAllDevices();
  public Task<DeviceOperationResult> AddDevice(DeviceBase device, Credentials credentials);
  public Task<DeviceOperationResult> UpdateDevice(uint id, DeviceBase device, Credentials credentials);
  public Task<DeviceOperationResult> RemoveDevice(uint id);
}