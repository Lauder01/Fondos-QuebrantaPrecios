using System;
using ClassLibraryProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingCompany> BuildingCompanies { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<FQP_User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apartment - Address (1:1)
            modelBuilder.Entity<Apartment>()
                .HasOne(a => a.ApartmentAddress)
                .WithOne(ad => ad.AddressApartment)
                .HasForeignKey<Apartment>(a => a.ApartmentAddressId);

            // Apartment - Floor (N:1)
            modelBuilder.Entity<Apartment>()
                .HasOne(a => a.ApartmentFloor)
                .WithMany(f => f.Apartments)
                .HasForeignKey(a => a.FloorId);

            // Apartment - Building (N:1)
            modelBuilder.Entity<Apartment>()
                .HasOne(a => a.ApartmentBuilding)
                .WithMany(b => b.Apartments)
                .HasForeignKey(a => a.BuildingId);

            // Address - Building (N:1)
            modelBuilder.Entity<Address>()
                .HasOne(a => a.AddressBuilding)
                .WithMany(b => b.BuildingAdress == null ? new List<Address>() : new List<Address> { b.BuildingAdress })
                .HasForeignKey(a => a.BuildingId);

            // Address - Apartment (N:1)
            modelBuilder.Entity<Address>()
                .HasOne(a => a.AddressApartment)
                .WithOne(ap => ap.ApartmentAddress)
                .HasForeignKey<Address>(a => a.ApartmentId);

            // Address - Street (N:1)
            modelBuilder.Entity<Address>()
                .HasOne(a => a.AddressStreet)
                .WithMany()
                .HasForeignKey(a => a.StreetId);

            // Building - District (N:1)
            modelBuilder.Entity<Building>()
                .HasOne(b => b.BuildingDistrict)
                .WithMany(d => d.Buildings)
                .HasForeignKey(b => b.DistrictId);

            // Building - Street (N:1)
            modelBuilder.Entity<Building>()
                .HasOne(b => b.BuildingStreet)
                .WithMany()
                .HasForeignKey(b => b.StreetId);

            // Building - Company (N:1)
            modelBuilder.Entity<Building>()
                .HasOne(b => b.BuildingCompany)
                .WithMany(c => c.CompanyBuildings)
                .HasForeignKey(b => b.CompanyId);

            // Building - Status (N:1)
            modelBuilder.Entity<Building>()
                .HasOne(b => b.BuildingStatus)
                .WithMany()
                .HasForeignKey(b => b.StatusId);

            // Building - Address (N:1)
            modelBuilder.Entity<Building>()
                .HasOne(b => b.BuildingAdress)
                .WithMany()
                .HasForeignKey(b => b.AddressId);

            // Floor - Building (N:1)
            modelBuilder.Entity<Floor>()
                .HasOne(f => f.BuildingFloor)
                .WithMany(b => b.FloorList)
                .HasForeignKey(f => f.BuildingId);

            // BuildingCompany - Address (N:1)
            modelBuilder.Entity<BuildingCompany>()
                .HasOne(c => c.CompanyAddress)
                .WithMany()
                .HasForeignKey(c => c.CompanyAddressId);

            // Request - Building (N:1)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestBuilding)
                .WithMany()
                .HasForeignKey(r => r.BuildingId);

            // Request - Status (N:1)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestStatus)
                .WithMany()
                .HasForeignKey(r => r.StatusId);
        }
    }
}
