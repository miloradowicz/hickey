namespace app.Models;

internal enum UserOperationResultCode
{
  UserAdded,
  UserUpdated,
  UserRemoved,
  UserAlreadyExists,
  UserNotFound,
  OperationFailed
}