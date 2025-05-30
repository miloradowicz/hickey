using shared.Models;

namespace api.Services;

internal interface IDeviceService
{
  public Task<DeviceReport> GetDeviceStatus(uint id);
  public Task<DeviceReport[]> GetAllDeviceStatuses();
  public Task<DeviceReport> RebootDevice(uint id);
  public Task<DeviceReport[]> RebootAllDevices();
}