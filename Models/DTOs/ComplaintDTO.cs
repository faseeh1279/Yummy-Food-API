using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.DTOs
{
    public class ComplaintDTO
    {
        public string ComplaintName { get; set; }
        public string ComplaintDescription { get; set; }
        public ComplaintStatus ComplaintStatus { get; set; }

        // Foreign keys
        public Guid? RiderProfileId { get; set; }
        public Guid? CustomerProfileId { get; set; }
    }
}
