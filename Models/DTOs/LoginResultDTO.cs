using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class LoginResultDTO
    {
        //public string? Message { get; set; } 
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; } 
        
    }
}
