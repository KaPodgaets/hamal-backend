using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<CitizenRecord> Citizens { get; set; }
    public DbSet<CallcenterCase> CallcenterCases { get; set; }

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

        modelBuilder.Entity<CallcenterCase>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.CallcenterCaseNumber)
                .IsRequired()
                .HasMaxLength(100);
            
            // Configure one-to-one relationship with unique index
            b.HasOne(c => c.CitizenRecord)
                .WithOne(c => c.CallcenterCase)
                .HasForeignKey<CallcenterCase>(c => c.CitizenRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Create unique index on CitizenRecordId to enforce one-to-one relationship
            b.HasIndex(c => c.CitizenRecordId)
                .IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
} 