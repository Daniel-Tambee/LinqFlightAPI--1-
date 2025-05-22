using ModelRoute = LinqFlightAPI.Models.Route;
using LinqFlightAPI.Models.Flight;

namespace LinqFlightAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Routes.Any()) return;

            var route1 = new ModelRoute { From = "A", To = "B", Duration = TimeSpan.FromHours(1.5) };
            var route2 = new ModelRoute { From = "B", To = "C", Duration = TimeSpan.FromHours(2) };
            var route3 = new ModelRoute { From = "A", To = "C", Duration = TimeSpan.FromHours(4) };

            context.Routes.AddRange(route1, route2, route3);
            context.SaveChanges();

            var flight1 = new Flight
            {
                RouteId = route1.Id,
                Provider = "Virgin",
                Price = 150,
                DepartureOffset = TimeSpan.FromDays(1) + TimeSpan.FromHours(2)
            };

            var flight2 = new Flight
            {
                RouteId = route2.Id,
                Provider = "Delta",
                Price = 200,
                DepartureOffset = TimeSpan.FromDays(2)
            };

            var flight3 = new Flight
            {
                RouteId = route3.Id,
                Provider = "JetBlue",
                Price = 250,
                DepartureOffset = TimeSpan.FromDays(1) + TimeSpan.FromHours(4)
            };

            context.Flights.AddRange(flight1, flight2, flight3);
            context.SaveChanges();
        }
    }
}
