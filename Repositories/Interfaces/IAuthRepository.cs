using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> SignUp(User user);
        Task<User?> Login(User user);
        Task<User?> GetUserAsync(string userEmail);
        Task<RefreshToken?> GetRefreshToken(string Token);
        Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
        //Task<User?> GetUserDataByRefreshToken(string refreshToken);
        Task<List<User>?> GetUsersAsync();
    }
}
