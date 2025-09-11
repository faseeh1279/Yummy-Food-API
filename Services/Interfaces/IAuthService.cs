using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDTO> Login(LoginDTO loginDTO);
        Task<string> SignUp(SignUpDTO signUpDTO);
        Task<string> GenerateNewAccessToken(string refreshToken); 
    }
}
