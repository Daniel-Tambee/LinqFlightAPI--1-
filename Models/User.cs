using System.ComponentModel.DataAnnotations;

namespace LinqFlightAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
