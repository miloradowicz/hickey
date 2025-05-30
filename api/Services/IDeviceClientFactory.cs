using api.Models;

namespace api.Services;

internal interface IDeviceClientFactory
{
  public HttpClient Create(Device device);
}