using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; } 
    }
}
