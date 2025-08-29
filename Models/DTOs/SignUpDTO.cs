using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.DTOs
{
    public class SignUpDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        
    }
}
