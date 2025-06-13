using app.Models;

internal record class UserOperationResult(
  UserOperationResultCode Result,
  User? User = null
);