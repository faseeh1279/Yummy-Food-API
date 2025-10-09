using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;
        public bool isBlocked { get; set; } = false;
    }
}
