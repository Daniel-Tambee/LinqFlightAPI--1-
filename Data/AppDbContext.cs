using LinqFlightAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LinqFlightAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<LinqFlightAPI.Models.Route> Routes => Set<LinqFlightAPI.Models.Route>();
        public DbSet<Flight> Flights => Set<Flight>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Route>()
                .HasMany(r => r.Flights)
                .WithOne(f => f.Route)
                .HasForeignKey(f => f.RouteId);
        }
    }
}
