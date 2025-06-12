using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Services;

internal class UserService(HickeyContext context) : IUserService
{
  private readonly HickeyContext context = context;

  async Task<UserOperationResult> IUserService.AddUser(User user)
  {
    if (await this.context.Users.FirstOrDefaultAsync(x => x.TelegramId == user.TelegramId || x.TelegramUsername == user.TelegramUsername) is not null)
    {
      return new UserOperationResult(UserOperationResultCode.UserExists, user);
    }

    User newUser = new(user);

    await this.context.Devices.AddAsync(newUser);

    try
    {
      await this.context.SaveChangesAsync();

      return new DeviceOperationResult(DeviceOperationResultCode.DeviceAdded, newUser);
    }
    catch (DbUpdateException)
    {
      return new DeviceOperationResult(DeviceOperationResultCode.OperationFailed);
    }
  }

  Task<UserOperationResult> IUserService.RemoveUser(uint id)
  {
    throw new NotImplementedException();
  }

  Task<UserOperationResult> IUserService.UpdateUser(uint id, User user)
  {
    throw new NotImplementedException();
  }
}