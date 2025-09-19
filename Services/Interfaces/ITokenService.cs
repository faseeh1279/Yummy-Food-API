using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJSONWebToken(LoginDTO loginDTO);
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string enteredPassword);
        string GenerateRefreshToken();
    }
}
