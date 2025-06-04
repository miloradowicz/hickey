using System.Xml.XPath;

namespace shared.Models;

public record class StatusOperationResult(
  OperationResultCode Result,
  DeviceBase? Device,
  DeviceStatusReport? Report
) : OperationResult(Result, Device);
