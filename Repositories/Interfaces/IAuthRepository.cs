using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> SignUp(User user);
    Task<User?> Login(User user);
    Task<RefreshToken?> GenerateRefreshToken(string Token);
}
