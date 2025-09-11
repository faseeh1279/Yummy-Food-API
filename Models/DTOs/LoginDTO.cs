using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; } 
        [Required]
        public string? Password { get; set; } 

    }
}
