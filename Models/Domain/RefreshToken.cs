using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.Domain
{
    public class RefreshToken
    {
        // Primary Key
        [Key]
        public Guid Id { get; set; } 
        // Foreign Key to User 
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty; 
        public DateTime Expires { get; set; } 
        public bool isExpired => DateTime.UtcNow >= Expires;
        public User User { get; set; } // Navigation property to the User entity
    }
}
