using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts
{
    public class CityInfoCintext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

        public CityInfoCintext(DbContextOptions<CityInfoCintext> options)
            : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("New Yourk City")
                {
                    Id = 1,
                    Description = "The one with that big park"
                },
                 new City("Antwerp")
                 {
                     Id = 2,
                     Description = "The one with that big park"
                 }, 
                 new City("New Yourk City")
                 {
                     Id = 3,
                     Description = "The one with that big park"
                 }
                );
            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The most visited urban park"
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "bla bla bla empire state"
                },
                new PointOfInterest("Cathedral")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "Cathedral bla bla"
                },
                 new PointOfInterest("SOmething ")
                 {
                     Id = 4,
                     CityId = 2,
                     Description = "Something "
                 },
                new PointOfInterest("Ephyl Tower")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "bla bla bla tower"
                },
                new PointOfInterest("The Louvre")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "The world largest museum"
                });
            base.OnModelCreating(modelBuilder);
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("connectionstring");
            base.OnConfiguring(optionsBuilder);
        }*/
    }
}
