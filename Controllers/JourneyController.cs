using LinqFlightAPI.Services;
using LinqFlightAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinqFlightAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JourneyController : ControllerBase
    {
        private readonly JourneyService _journeyService;

        public JourneyController(JourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpGet("minimize-exchanges")]
        public async Task<ActionResult<List<JourneyResultDto>>> MinimizeExchanges(
            string origin, string destination, bool ascending = true)
        {
            var journeys = await _journeyService.GetJourneysMinExchanges(origin, destination, ascending);
            if (journeys.Count == 0) return NotFound("No valid journey found.");
            return Ok(journeys);
        }

        // Task 2: Minimize Price
        [HttpGet("minimize-price")]
        public async Task<ActionResult<List<JourneyResultDto>>> MinimizePrice(
            string origin, string destination, string departure, bool ascending = true)
        {
            if (!TryParseOffset(departure, out var offset))
                return BadRequest("Invalid departure format. Example: +2 day 4 hour");

            var journeys = await _journeyService.GetJourneysMinPrice(origin, destination, offset, ascending);
            if (journeys.Count == 0) return NotFound("No valid journey found.");
            return Ok(journeys);
        }

        // Task 3: Minimize Duration
        [HttpGet("minimize-duration")]
        public async Task<ActionResult<List<JourneyResultDto>>> MinimizeDuration(
            string origin, string destination, string departure, bool ascending = true)
        {
            if (!TryParseOffset(departure, out var offset))
                return BadRequest("Invalid departure format. Example: +2 day 4 hour");

            var journeys = await _journeyService.GetJourneysMinDuration(origin, destination, offset, ascending);
            if (journeys.Count == 0) return NotFound("No valid journey found.");
            return Ok(journeys);
        }

        // Task 4: All journeys with pagination
        [HttpGet("all")]
        public async Task<ActionResult<object>> GetAllJourneys(
            int page = 1, int size = 100, bool ascending = true)
        {
            var (journeys, totalCount) = await _journeyService.GetAllJourneys(page, size, ascending);
            return Ok(new
            {
                TotalJourneys = totalCount,
                CurrentPage = page,
                PageSize = size,
                Results = journeys
            });
        }

          private bool TryParseOffset(string input, out TimeSpan offset)
        {
            offset = TimeSpan.Zero;
            try
            {
                var parts = input.Trim('+', ' ').Split(" ");
                int days = 0, hours = 0;
                for (int i = 0; i < parts.Length; i += 2)
                {
                    int value = int.Parse(parts[i]);
                    if (parts[i + 1].StartsWith("day")) days = value;
                    else if (parts[i + 1].StartsWith("hour")) hours = value;
                }
                offset = TimeSpan.FromDays(days) + TimeSpan.FromHours(hours);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
