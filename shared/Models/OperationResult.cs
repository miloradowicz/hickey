namespace shared.Models;

public record class OperationResult(
  OperationResultCode Result,
  DeviceBase? Device
);
