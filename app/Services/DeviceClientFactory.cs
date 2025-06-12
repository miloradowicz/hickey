using System.Net;
using app.Models;

namespace app.Services;

internal class DeviceClientFactory : IDeviceClientFactory
{
  public HttpClient Create(Device device)
  {
    return new(new HttpClientHandler()
    {
      Credentials = new NetworkCredential()
      {
        UserName = device.Credentials.Username,
        Password = device.Credentials.Password,
      }
    })
    {
      BaseAddress = new Uri($"http://{device.Address}:{device.Port}/ISAPI/")
    };
  }
}