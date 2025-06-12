using System.Xml.Serialization;

namespace app.Responses;

[XmlRoot(ElementName = "DeviceStatus", Namespace = "http://www.hikvision.com/ver20/XMLSchema")]
public class StatusResponse
{

  [XmlElement(ElementName = "currentDeviceTime")]
  public required DateTime DeviceTime { get; init; }

  [XmlElement(ElementName = "deviceUpTime")]
  public required ulong DeviceUptime { get; init; }
}
