using Microsoft.EntityFrameworkCore;

namespace bot.Models;

internal class BotContext(DbContextOptions<BotContext> options) : DbContext(options)
{
  public DbSet<DialogState> DialogState { get; init; }
}
