using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string HashedPassword { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;

    }
}
