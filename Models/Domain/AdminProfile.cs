using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.Domain
{
    public class AdminProfile
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } // Foreign key to User

        // Navigation properties
        public User User { get; set; }
        
        
    }
}
