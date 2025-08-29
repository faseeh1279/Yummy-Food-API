using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> SignUp(User user);
        Task<(bool, string)> Login(User user);
        Task<string> GetUserAsync(User user);
        //Task SaveRefreshToken();
    }
}
