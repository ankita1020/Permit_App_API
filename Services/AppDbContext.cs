namespace Permit_App.Services
{
    using Microsoft.EntityFrameworkCore;
    using Permit_App.Models;
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<County> Counties { get; set; }
        public DbSet<PermitType> PermitTypes { get; set; }
        public DbSet<FormData> FormSubmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormData>()
                .HasKey(form => form.Id);

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
        }
    }
}
