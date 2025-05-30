using api.Models;
using shared.Models;
using System.Net;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using api.Exceptions;

namespace api.Services;

internal partial class DeviceService(
  ApiContext context,
  IDeviceClientFactory clientFactory
  ) : IDeviceService
{
  private readonly ApiContext context = context;
  private readonly IDeviceClientFactory clientFactory = clientFactory;

  public Task<DeviceReport[]> GetAllDeviceStatuses()
  {
    throw new NotImplementedException();
  }

  public async Task<DeviceReport> GetDeviceStatus(uint id)
  {
    var device = await this.context.Devices.FirstAsync(x => x.Id == id) ?? throw new DeviceNotFoundException();

    using var client = this.clientFactory.Create(device);

    var response = await client.GetAsync(Endpoint.Status);

    if (response.StatusCode == HttpStatusCode.OK)
    {
      var serializer = new XmlSerializer(typeof(Responses.StatusResponse));

      var statusResponse = serializer.Deserialize(await response.Content.ReadAsStreamAsync()) as Responses.StatusResponse ?? throw new InvalidResponseException();

      return new DeviceReport(device.Name, DeviceStatus.Up, device.LastReboot, statusResponse.DeviceUptime);
    }
    else
    {
      return new DeviceReport(device.Name, DeviceStatus.Unknown, device.LastReboot, null);
    }
  }

  public Task<DeviceReport[]> RebootAllDevices()
  {
    throw new NotImplementedException();
  }

  public Task<DeviceReport> RebootDevice(uint id)
  {
    throw new NotImplementedException();
  }
}