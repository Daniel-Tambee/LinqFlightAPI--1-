namespace LinqFlightAPI.DTOs
{
    public class JourneyResultDto
    {
        public List<string> Locations { get; set; } = new();
        public List<string> Providers { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public TimeSpan DepartureOffset { get; set; }  // Earliest departure
    }
}
