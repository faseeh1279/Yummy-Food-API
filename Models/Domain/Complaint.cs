using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.Domain
{
    public class Complaint
    {
        [Key]
        public Guid Id { get; set; } 
        public string ComplaintName { get; set; }
        public string ComplaintDescription { get; set; }
        public ComplaintStatus ComplaintStatus { get; set; } = ComplaintStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt {  get; set; } = DateTime.UtcNow;

        // Foreign keys
        public Guid? RiderProfileId { get; set; }
        public RiderProfile? RiderProfile { get; set; }
        public Guid? CustomerProfileId { get; set; }
        public CustomerProfile? CustomerProfile { get; set; }
    }
}
