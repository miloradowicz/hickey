using app.Models;

namespace app.Services;

internal interface IUserService
{
  public Task<UserOperationResult> AddUser(User user);
  public Task<UserOperationResult> UpdateUser(uint id, User user);
  public Task<UserOperationResult> RemoveUser(uint id);
}