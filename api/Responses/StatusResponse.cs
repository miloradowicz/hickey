using System.Xml.Serialization;

namespace api.Responses;

[XmlRoot(ElementName = "DeviceStatus", Namespace = "http://www.hikvision.com/ver20/XMLSchema")]
public class StatusResponse
{

  [XmlElement(ElementName = "currentDeviceTime")]
  public DateTime DeviceTime { get; set; }

  [XmlElement(ElementName = "deviceUpTime")]
  public ulong DeviceUptime { get; set; }
}
