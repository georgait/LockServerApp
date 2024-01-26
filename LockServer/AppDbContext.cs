using Microsoft.EntityFrameworkCore;

namespace LockServer;

public class AppDbContext : DbContext
{
    public DbSet<Runner> Runners { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }
}
