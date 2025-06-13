using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Services;

internal class UserService(HickeyContext context) : IUserService
{
  private readonly HickeyContext context = context;

  public async Task<UserOperationResult> AddUser(User user)
  {
    if (await this.context.Users.FirstOrDefaultAsync(x => x.TelegramId == user.TelegramId) is not null)
    {
      return new UserOperationResult(UserOperationResultCode.UserAlreadyExists, user);
    }

    User newUser = new(user);

    await this.context.Users.AddAsync(newUser);

    try
    {
      await this.context.SaveChangesAsync();

      return new UserOperationResult(UserOperationResultCode.UserAdded, newUser);
    }
    catch (DbUpdateException)
    {
      return new UserOperationResult(UserOperationResultCode.OperationFailed);
    }
  }

  public async Task<UserOperationResult> UpdateUser(uint id, User user)
  {
    if (await this.context.Users.FirstOrDefaultAsync(x => x.TelegramId == user.TelegramId) is not null)
    {
      return new UserOperationResult(UserOperationResultCode.UserAlreadyExists, user);
    }

    var existingUser = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

    if (existingUser is null)
    {
      return new UserOperationResult(UserOperationResultCode.UserNotFound);
    }

    (existingUser.Name, existingUser.TelegramId) = user;

    try
    {
      await this.context.SaveChangesAsync();

      return new UserOperationResult(UserOperationResultCode.UserUpdated, existingUser);
    }
    catch (DbUpdateException)
    {
      return new UserOperationResult(UserOperationResultCode.OperationFailed);
    }
  }

  public async Task<UserOperationResult> RemoveUser(uint id)
  {
    var existingUser = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

    if (existingUser is null)
    {
      return new UserOperationResult(UserOperationResultCode.UserNotFound);
    }

    this.context.Users.Remove(existingUser);

    try
    {
      await this.context.SaveChangesAsync();

      return new UserOperationResult(UserOperationResultCode.UserAdded, existingUser);
    }
    catch (DbUpdateException)
    {
      return new UserOperationResult(UserOperationResultCode.OperationFailed);
    }
  }

  public async Task<User?> FindUserByTelegramId(long telegramId)
  {
    var existingUser = await this.context.Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);

    if (existingUser is null)
    {
      return null;
    }

    return existingUser;
  }
}