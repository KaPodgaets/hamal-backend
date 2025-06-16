using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Citizen> Citizens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Role)
                .HasConversion<string>();
        });

        modelBuilder.Entity<Citizen>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Status)
                .HasConversion<string>();
            
            // Example of seeding data for testing. 
            // In a real scenario, this would come from a data import.
            b.HasData(new Citizen { Id = 1, StreetName = "הרצל", BuildingNumber = "10", FlatNumber = "5", FirstName="ישראל", LastName="ישראלי", FamilyNumber=4, Status=CitizenStatus.Pending, LastUpdatedAt=DateTime.UtcNow});
            b.HasData(new Citizen { Id = 2, StreetName = "ז'בוטינסקי", BuildingNumber = "22", FlatNumber = "1", FirstName="משה", LastName="כהן", FamilyNumber=2, Status=CitizenStatus.Pending, IsLonely=true, LastUpdatedAt=DateTime.UtcNow });
        });

        base.OnModelCreating(modelBuilder);
    }
} 