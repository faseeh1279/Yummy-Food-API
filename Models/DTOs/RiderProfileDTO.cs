using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class RiderProfileDTO
    {
        [Required]
        public string Cnic { get; set; }
    }
}
