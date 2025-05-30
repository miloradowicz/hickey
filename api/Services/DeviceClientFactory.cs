using System.Net;
using api.Models;

namespace api.Services;

internal class DeviceClientFactory : IDeviceClientFactory
{
  public HttpClient Create(Device device)
  {
    return new(new HttpClientHandler()
    {
      Credentials = new NetworkCredential()
      {
        UserName = device.Username,
        Password = device.Password,
      }
    })
    {
      BaseAddress = new Uri($"http://{device.Address}:{device.Port}/ISAPI/")
    };
  }
}