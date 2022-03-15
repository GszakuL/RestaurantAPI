using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDAL.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString = 
            "Server = (localdb)\\mssqllocaldb;Database=RestaurantDb;Trusted_Connection=True;";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(a => a.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(a => a.Name)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
