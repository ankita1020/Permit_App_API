namespace Permit_App.Services
{
    using Microsoft.EntityFrameworkCore;
    using Permit_App.Models;
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<County> Counties { get; set; }
        public DbSet<PermitType> PermitTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>().HasData(
                new County { CountyId = 1, CountyName = "Hillsborough" },
                new County { CountyId = 2, CountyName = "Pasco" },
                new County { CountyId = 3, CountyName = "Pinellas" },
                new County { CountyId = 4, CountyName = "Orange" },
                new County { CountyId = 5, CountyName = "Polk" }
            );

            modelBuilder.Entity<PermitType>().HasData(
                new PermitType { PermitTypeId = 1, PermitTypeName = "Water Use Permit" },
                new PermitType { PermitTypeId = 2, PermitTypeName = "Environmental Resource Type" }
            );

            modelBuilder.Entity<Address>()
            .HasIndex(a => new { a.AddressLine1, a.City, a.State, a.ZipCode })
            .IsUnique(); // Prevent duplicate addresses

            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<User>(u => u.AddressId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete if needed
        }
    }
}
