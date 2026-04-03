using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.Domain
{
    public class RiderProfile
    {
        [Key]
        public Guid Id { get; set;  }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Cnic { get; set; }

        // Navigation Property
        public User User { get; set; }
        // A rider can have many orders
        public ICollection<Order> Orders { get; set; }
        public ICollection<Complaint> Complaints { get; set; } 

    }
}
