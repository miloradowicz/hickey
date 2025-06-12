using System.Net;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using app.Models;
using app.Endpoints;

namespace app.Services;

internal partial class DeviceService(
  HickeyContext context,
  IDeviceClientFactory clientFactory
  ) : IDeviceService
{
  private readonly HickeyContext context = context;
  private readonly IDeviceClientFactory clientFactory = clientFactory;
  private readonly XmlSerializer serializer = new(typeof(Responses.StatusResponse));

  private async Task<DeviceStatusOperationResult> RequestDeviceStatus(Device device)
  {
    using var client = this.clientFactory.Create(device);

    try
    {
      var result = await client.GetAsync(HikvisionEndpoint.Status);

      if (result.StatusCode == HttpStatusCode.OK)
      {
        var response = this.serializer.Deserialize(await result.Content.ReadAsStreamAsync()) as Responses.StatusResponse;

        return response is not null
          ? new DeviceStatusOperationResult(DeviceOperationResultCode.StatusReceived, device, new DeviceStatusReport(DeviceStatusCode.Up, response.DeviceUptime))
          : new DeviceStatusOperationResult(DeviceOperationResultCode.BadResponse, device);
      }
      else if (result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.Forbidden)
      {
        return new DeviceStatusOperationResult(DeviceOperationResultCode.AccessDenied, device);
      }
      else
      {
        return new DeviceStatusOperationResult(DeviceOperationResultCode.BadResponse, device);
      }
    }
    catch (InvalidOperationException)
    {
      return new DeviceStatusOperationResult(DeviceOperationResultCode.BadConfiguration);
    }
    catch (HttpRequestException)
    {
      return new DeviceStatusOperationResult(DeviceOperationResultCode.NoResponse);
    }
  }

  private async Task<DeviceOperationResult> RequestDeviceReboot(Device device)
  {
    using var client = this.clientFactory.Create(device);

    try
    {
      var result = await client.PutAsync(HikvisionEndpoint.Reboot, null);

      if (result.StatusCode == HttpStatusCode.OK)
      {
        var response = this.serializer.Deserialize(await result.Content.ReadAsStreamAsync()) as Responses.Response;

        return response is not null
          ? new DeviceOperationResult(DeviceOperationResultCode.RebootRequested, device)
          : new DeviceOperationResult(DeviceOperationResultCode.BadResponse, device);
      }
      else if (result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.Forbidden)
      {
        return new DeviceOperationResult(DeviceOperationResultCode.AccessDenied, device);
      }
      else
      {
        return new DeviceOperationResult(DeviceOperationResultCode.BadResponse, device); ;
      }
    }
    catch (InvalidOperationException)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.BadConfiguration, device);
    }
    catch (HttpRequestException)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.NoResponse, device);
    }
  }

  public async Task<DeviceStatusOperationResult> GetDeviceStatus(uint id)
  {
    var device = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == id);

    if (device is null)
    {
      return new DeviceStatusOperationResult(DeviceOperationResultCode.DeviceNotFound);
    }

    return await this.RequestDeviceStatus(device);
  }

  public async Task<DeviceOperationResult> RebootDevice(uint id)
  {
    var device = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == id);

    if (device is null)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.DeviceNotFound);
    }

    return await this.RequestDeviceReboot(device);
  }

  public async Task<DeviceStatusOperationResult[]> GetAllDeviceStatuses()
  {
    List<Task<DeviceStatusOperationResult>> requests = [];

    foreach (var device in this.context.Devices)
    {
      requests.Add(this.RequestDeviceStatus(device));
    }

    return await Task.WhenAll(requests);
  }

  public async Task<DeviceOperationResult[]> RebootAllDevices()
  {
    List<Task<DeviceOperationResult>> requests = [];

    foreach (var device in this.context.Devices)
    {
      requests.Add(this.RequestDeviceReboot(device));
    }

    return await Task.WhenAll(requests);
  }

  public async Task<DeviceOperationResult> AddDevice(DeviceBase device, Credentials credentials)
  {
    if (await this.context.Devices.FirstOrDefaultAsync(x => x.Address == device.Address && x.Port == device.Port) is not null)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.DeviceExists, device);
    }

    Device newDevice = new(device, credentials);

    await this.context.Devices.AddAsync(newDevice);

    try
    {
      await this.context.SaveChangesAsync();

      return new DeviceOperationResult(DeviceOperationResultCode.DeviceAdded, newDevice);
    }
    catch (DbUpdateException)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.OperationFailed);
    }
  }

  public async Task<DeviceOperationResult> UpdateDevice(uint id, DeviceBase device, Credentials credentials)
  {
    if (await this.context.Devices.FirstOrDefaultAsync(x => x.Address == device.Address && x.Port == device.Port) is not null)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.DeviceExists, device);
    }

    var existingDevice = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == id);

    if (existingDevice is null)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.DeviceNotFound);
    }

    (existingDevice.Name, existingDevice.Address, existingDevice.Port) = device;
    (existingDevice.Credentials.Username, existingDevice.Credentials.Password) = credentials;

    try
    {
      await this.context.SaveChangesAsync();

      return new DeviceOperationResult(DeviceOperationResultCode.DeviceUpdated, existingDevice);
    }
    catch (DbUpdateException)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.OperationFailed);
    }
  }

  public async Task<DeviceOperationResult> RemoveDevice(uint id)
  {
    var device = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == id);

    if (device is null)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.DeviceNotFound);
    }

    this.context.Devices.Remove(device);

    try
    {
      await this.context.SaveChangesAsync();

      return new DeviceOperationResult(DeviceOperationResultCode.DeviceAdded, device);
    }
    catch (DbUpdateException)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.OperationFailed);
    }
  }
}