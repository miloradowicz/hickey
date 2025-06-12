namespace app.Models;

internal enum UserOperationResultCode
{
  UserAdded,
  UserUpdated,
  UserRemoved,
  UserExists,
  UserNotFound,
  OperationFailed
}