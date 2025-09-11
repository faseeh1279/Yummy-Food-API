using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.Domain
{
    public class CustomerProfile
    {
        [Key]
        public Guid Id { get; set;  }
        public Guid UserId { get; set; }  // FK to User
        public ComplaintStatus ComplaintStatus { get; set; } = ComplaintStatus.Pending;
        // Navigation
        public User User { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        //public Order Order { get; set; } 
        public ICollection<Order> Orders { get; set; } 


    }
}
