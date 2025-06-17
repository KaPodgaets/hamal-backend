using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<CitizenRecord> Citizens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Role)
                .HasConversion<string>();
        });

        modelBuilder.Entity<CitizenRecord>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.StatusInCallCenter)
                .HasConversion<string>();
        });

        base.OnModelCreating(modelBuilder);
    }
} 