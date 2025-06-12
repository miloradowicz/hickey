using app.Models;

namespace app.Services;

internal interface IDeviceClientFactory
{
  public HttpClient Create(Device device);
}