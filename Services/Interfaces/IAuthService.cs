using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResultDTO>> Login(LoginDTO loginDTO);
        Task<ServiceResponse<User>> SignUp(SignUpDTO signUpDTO);
        Task<ServiceResponse<string>> GenerateNewAccessToken(string refreshToken); 
    }
}
