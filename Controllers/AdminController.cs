using LinqFlightAPI.Data;
using LinqFlightAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinqFlightAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (_context.Routes.Any()) return BadRequest("Data already exists.");

            var route = new Route { From = "A", To = "C", Duration = TimeSpan.FromHours(2) };
            _context.Routes.Add(route);

            var flight = new Flight
            {
                Route = route,
                Provider = "Virgin",
                Price = 100,
                DepartureOffset = TimeSpan.FromDays(4) + TimeSpan.FromHours(5)
            };
            _context.Flights.Add(flight);

            await _context.SaveChangesAsync();
            return Ok("Seeded sample data.");
        }
    }
}
