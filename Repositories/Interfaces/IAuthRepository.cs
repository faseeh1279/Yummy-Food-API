using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> SignUp(User user);
        Task<(bool, string, Guid)> Login(User user);
        Task<User> GetUserAsync(User user);
        Task<RefreshToken?> GetRefreshToken(string Token); 
        Task AddRefreshToken(RefreshToken refreshToken);
        Task<User?> GetUserDataByRefreshToken(string refreshToken);
        Task<List<User>> GetUsersAsync();
    }
}
