using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJSONWebToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string enteredPassword);
        string GenerateRefreshToken();
    }
}
