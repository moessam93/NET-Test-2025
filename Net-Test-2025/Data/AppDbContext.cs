using Microsoft.EntityFrameworkCore;
using Net_Test_2025.Domains;

namespace Net_Test_2025.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Client
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Phone)
            .IsUnique();

        modelBuilder.Entity<Client>().Property(c => c.Gender).HasConversion<string>();

        modelBuilder.Entity<Client>().HasQueryFilter(c => !c.Deleted);
        #endregion
    }
} 