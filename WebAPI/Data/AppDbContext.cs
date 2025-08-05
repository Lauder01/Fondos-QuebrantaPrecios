using Microsoft.EntityFrameworkCore;
using FQP.Entities;

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
    }
}
