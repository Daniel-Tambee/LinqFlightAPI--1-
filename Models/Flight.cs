using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqFlightAPI.Models
{
    public class Flight
    {
        public int Id { get; set; }

        [Required]
        public string Provider { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public TimeSpan DepartureOffset { get; set; }

        [Required]
        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        public Route Route { get; set; } = null!;
    }
}
