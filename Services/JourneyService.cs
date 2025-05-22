using LinqFlightAPI.Data;
using LinqFlightAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LinqFlightAPI.Services
{
    public class JourneyService
    {
        private readonly AppDbContext _db;

        public JourneyService(AppDbContext db)
        {
            _db = db;
        }

        // === Task 1 ===
        public async Task<List<JourneyResultDto>> GetJourneysMinExchanges(string origin, string destination, bool ascending)
        {
            var routes = await _db.Routes.Include(r => r.Flights).ToListAsync();
            var journeys = new List<List<(string from, string to, string provider, decimal price, TimeSpan duration, TimeSpan departure)>>();

            void DFS(string current, string target, HashSet<string> visited, List<(string, string, string, decimal, TimeSpan, TimeSpan)> path)
            {
                if (visited.Contains(current)) return;
                visited.Add(current);

                if (current == target)
                {
                    journeys.Add(new(path));
                    visited.Remove(current);
                    return;
                }

                foreach (var route in routes.Where(r => r.From == current))
                {
                    foreach (var flight in route.Flights)
                    {
                        path.Add((route.From, route.To, flight.Provider, flight.Price, route.Duration, flight.DepartureOffset));
                        DFS(route.To, target, visited, path);
                        path.RemoveAt(path.Count - 1);
                    }
                }

                visited.Remove(current);
            }

            DFS(origin, destination, new(), new());

            var result = journeys
                .Select(j => new JourneyResultDto
                {
                    Locations = j.Select(s => s.from).Append(j.Last().to).ToList(),
                    Providers = j.Select(s => s.provider).ToList(),
                    TotalPrice = j.Sum(s => s.price),
                    TotalDuration = new TimeSpan(j.Sum(s => s.duration.Ticks)),
                    DepartureOffset = j.First().departure
                }).ToList();

            return ascending
                ? result.OrderBy(r => r.Locations.Count).ToList()
                : result.OrderByDescending(r => r.Locations.Count).ToList();
        }

        // === Task 2 ===
        public async Task<List<JourneyResultDto>> GetJourneysMinPrice(string origin, string destination, TimeSpan minDepartureOffset, bool ascending)
        {
            var allRoutes = await _db.Routes.Include(r => r.Flights).ToListAsync();
            var journeys = new List<List<(string from, string to, string provider, decimal price, TimeSpan duration, TimeSpan departure)>>();

            void DFS(string current, string target, HashSet<string> visited, List<(string, string, string, decimal, TimeSpan, TimeSpan)> path)
            {
                if (visited.Contains(current)) return;
                visited.Add(current);

                if (current == target)
                {
                    if (path.First().Item6 >= minDepartureOffset)
                        journeys.Add(new(path));
                    visited.Remove(current);
                    return;
                }

                foreach (var route in allRoutes.Where(r => r.From == current))
                {
                    foreach (var flight in route.Flights)
                    {
                        path.Add((route.From, route.To, flight.Provider, flight.Price, route.Duration, flight.DepartureOffset));
                        DFS(route.To, target, visited, path);
                        path.RemoveAt(path.Count - 1);
                    }
                }

                visited.Remove(current);
            }

            DFS(origin, destination, new(), new());

            var result = journeys.Select(j => new JourneyResultDto
            {
                Locations = j.Select(s => s.from).Append(j.Last().to).ToList(),
                Providers = j.Select(s => s.provider).ToList(),
                TotalPrice = j.Sum(s => s.price),
                TotalDuration = new TimeSpan(j.Sum(s => s.duration.Ticks)),
                DepartureOffset = j.First().departure
            }).ToList();

            return ascending
                ? result.OrderBy(r => r.TotalPrice).ToList()
                : result.OrderByDescending(r => r.TotalPrice).ToList();
        }

        // === Task 3 ===
        public async Task<List<JourneyResultDto>> GetJourneysMinDuration(string origin, string destination, TimeSpan minDepartureOffset, bool ascending)
        {
            var allRoutes = await _db.Routes.Include(r => r.Flights).ToListAsync();
            var journeys = new List<List<(string from, string to, string provider, decimal price, TimeSpan duration, TimeSpan departure)>>();

            void DFS(string current, string target, HashSet<string> visited, List<(string, string, string, decimal, TimeSpan, TimeSpan)> path)
            {
                if (visited.Contains(current)) return;
                visited.Add(current);

                if (current == target)
                {
                    if (path.First().Item6 >= minDepartureOffset)
                        journeys.Add(new(path));
                    visited.Remove(current);
                    return;
                }

                foreach (var route in allRoutes.Where(r => r.From == current))
                {
                    foreach (var flight in route.Flights)
                    {
                        path.Add((route.From, route.To, flight.Provider, flight.Price, route.Duration, flight.DepartureOffset));
                        DFS(route.To, target, visited, path);
                        path.RemoveAt(path.Count - 1);
                    }
                }

                visited.Remove(current);
            }

            DFS(origin, destination, new(), new());

            var result = journeys.Select(j => new JourneyResultDto
            {
                Locations = j.Select(s => s.from).Append(j.Last().to).ToList(),
                Providers = j.Select(s => s.provider).ToList(),
                TotalPrice = j.Sum(s => s.price),
                TotalDuration = new TimeSpan(j.Sum(s => s.duration.Ticks)),
                DepartureOffset = j.First().departure
            }).ToList();

            return ascending
                ? result.OrderBy(r => r.TotalDuration).ToList()
                : result.OrderByDescending(r => r.TotalDuration).ToList();
        }

        // === Task 4 ===
        public async Task<(List<JourneyResultDto> Journeys, int TotalCount)> GetAllJourneys(int page, int size, bool ascending)
        {
            var routes = await _db.Routes.Include(r => r.Flights).ToListAsync();
            var allJourneys = new List<JourneyResultDto>();

            void DFS(string current, List<(string, string, string, decimal, TimeSpan, TimeSpan)> path, HashSet<string> visited)
            {
                if (visited.Contains(current)) return;
                visited.Add(current);

                foreach (var route in routes.Where(r => r.From == current))
                {
                    foreach (var flight in route.Flights)
                    {
                        path.Add((route.From, route.To, flight.Provider, flight.Price, route.Duration, flight.DepartureOffset));

                        var journey = new JourneyResultDto
                        {
                            Locations = path.Select(p => p.Item1).Append(path.Last().Item2).ToList(),
                            Providers = path.Select(p => p.Item3).ToList(),
                            TotalPrice = path.Sum(p => p.Item4),
                            TotalDuration = new TimeSpan(path.Sum(p => p.Item5.Ticks)),
                            DepartureOffset = path.First().Item6
                        };

                        allJourneys.Add(journey);

                        DFS(route.To, path, visited);
                        path.RemoveAt(path.Count - 1);
                    }
                }

                visited.Remove(current);
            }

            foreach (var start in routes.Select(r => r.From).Distinct())
                DFS(start, new(), new());

            var ordered = ascending
                ? allJourneys.OrderBy(j => j.TotalPrice)
                : allJourneys.OrderByDescending(j => j.TotalPrice);

            var paginated = ordered.Skip((page - 1) * size).Take(size).ToList();
            return (paginated, allJourneys.Count);
        }
    }
}
