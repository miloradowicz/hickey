using System.Xml.Serialization;

namespace api.Responses;

[XmlRoot(ElementName = "ResponseStatus", Namespace = "urn:psialliance-org")]
public class Response
{
  [XmlElement(ElementName = "requestURL")]
  public required string RequestUrl { get; init; }

  [XmlElement(ElementName = "statusCode")]
  public required int StatusCode { get; init; }

  [XmlElement(ElementName = "statusString")]
  public required string StatusString { get; init; }

  [XmlElement(ElementName = "subStatusCode")]
  public required string StatusSubcode { get; init; }
}