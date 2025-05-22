using LinqFlightAPI.Models;
using Microsoft.EntityFrameworkCore;
using ModelRoute = LinqFlightAPI.Models.Route;
using ModelFlight = LinqFlightAPI.Models.Flight;

namespace LinqFlightAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<ModelRoute> Routes => Set<ModelRoute>();
        public DbSet<ModelFlight> Flights => Set<ModelFlight>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ModelRoute>()
                .HasMany(r => r.Flights)
                .WithOne(f => f.Route)
                .HasForeignKey(f => f.RouteId);
        }
    }
}
