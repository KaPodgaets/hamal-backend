using Hamal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Role)
                .HasConversion<string>();
        });

        base.OnModelCreating(modelBuilder);
    }
} 