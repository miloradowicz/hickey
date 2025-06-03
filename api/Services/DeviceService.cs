using System.Net;
using System.Xml.Serialization;
using api.Models;
using shared.Models;
using Microsoft.EntityFrameworkCore;
using api.Endpoints;

namespace api.Services;

internal partial class DeviceService(
  ApiContext context,
  IDeviceClientFactory clientFactory
  ) : IDeviceService
{
  private readonly ApiContext context = context;
  private readonly IDeviceClientFactory clientFactory = clientFactory;
  private readonly XmlSerializer serializer = new(typeof(Responses.StatusResponse));

  private async Task<DeviceReport> RequestDeviceStatus(Device device)
  {
    using var client = this.clientFactory.Create(device);

    try
    {
      var result = await client.GetAsync(HikvisionEndpoint.Status);

      if (result.StatusCode == HttpStatusCode.OK)
      {
        var response = this.serializer.Deserialize(await result.Content.ReadAsStreamAsync()) as Responses.StatusResponse;

        return response is not null
          ? new DeviceReport(device.Name, DeviceStatus.Up, response.DeviceUptime)
          : new DeviceReport(device.Name, DeviceStatus.CommunicationFailed, null);
      }
      else if (result.StatusCode == HttpStatusCode.Unauthorized)
      {
        return new DeviceReport(device.Name, DeviceStatus.Denied, null);
      }
      else
      {
        return new DeviceReport(device.Name, DeviceStatus.CommunicationFailed, null); ;
      }
    }
    catch (InvalidOperationException)
    {
      return new DeviceReport(device.Name, DeviceStatus.CommunicationFailed, null);
    }
    catch (HttpRequestException)
    {
      return new DeviceReport(device.Name, DeviceStatus.Down, null);
    }
  }

  private async Task<DeviceReport> RequestDeviceReboot(Device device)
  {
    using var client = this.clientFactory.Create(device);

    try
    {
      var result = await client.PutAsync(HikvisionEndpoint.Reboot, null);

      if (result.StatusCode == HttpStatusCode.OK)
      {
        var response = this.serializer.Deserialize(await result.Content.ReadAsStreamAsync()) as Responses.Response;

        return response is not null
          ? new DeviceReport(device.Name, DeviceStatus.RebootPending, null)
          : new DeviceReport(device.Name, DeviceStatus.CommunicationFailed, null);
      }
      else if (result.StatusCode == HttpStatusCode.Unauthorized)
      {
        return new DeviceReport(device.Name, DeviceStatus.Denied, null);
      }
      else
      {
        return new DeviceReport(device.Name, DeviceStatus.CommunicationFailed, null); ;
      }
    }
    catch (InvalidOperationException)
    {
      return new DeviceReport(device.Name, DeviceStatus.CommunicationFailed, null);
    }
    catch (HttpRequestException)
    {
      return new DeviceReport(device.Name, DeviceStatus.Down, null);
    }
  }

  public async Task<DeviceReport?> GetDeviceStatus(uint id)
  {
    var device = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == id);

    if (device is null)
    {
      return null;
    }

    return await this.RequestDeviceStatus(device);
  }

  public async Task<DeviceReport?> RebootDevice(uint id)
  {
    var device = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == id);

    if (device is null)
    {
      return null;
    }

    var status = await this.RequestDeviceStatus(device);

    return status.Status == DeviceStatus.Up
      ? await this.RequestDeviceReboot(device)
      : status;
  }

  public async Task<DeviceReport[]> GetAllDeviceStatuses()
  {
    List<Task<DeviceReport>> requests = [];

    foreach (var device in this.context.Devices)
    {
      requests.Add(this.RequestDeviceStatus(device));
    }

    return await Task.WhenAll(requests);
  }

  public async Task<DeviceReport[]> RebootAllDevices()
  {
    List<Task<DeviceReport>> requests = [];

    foreach (var device in this.context.Devices)
    {
      async Task<DeviceReport> request()
      {
        var status = await this.RequestDeviceStatus(device);

        return status.Status == DeviceStatus.Up
          ? await this.RequestDeviceReboot(device)
          : status;
      }

      requests.Add(request());
    }

    return await Task.WhenAll(requests);
  }
}