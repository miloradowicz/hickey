using Microsoft.EntityFrameworkCore;

namespace api.Models;

internal class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
  public DbSet<Device> Devices { get; init; }
  public DbSet<Configuration> Configuration { get; init; }
}
