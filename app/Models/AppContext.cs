using Microsoft.EntityFrameworkCore;

namespace app.Models;

internal class HickeyContext(DbContextOptions<HickeyContext> options) : DbContext(options)
{
  public DbSet<Device> Devices { get; init; }
  public DbSet<User> Users { get; init; }
  public DbSet<Configuration> Configuration { get; init; }
  public DbSet<DialogState> DialogState { get; init; }
}
