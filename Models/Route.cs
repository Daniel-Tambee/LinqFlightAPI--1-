using System.ComponentModel.DataAnnotations;

namespace LinqFlightAPI.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Required]
        public string From { get; set; } = string.Empty;

        [Required]
        public string To { get; set; } = string.Empty;

        [Required]
        public TimeSpan Duration { get; set; }

        public ICollection<Flight> Flights { get; set; } = new List<Flight>();
    }
}
