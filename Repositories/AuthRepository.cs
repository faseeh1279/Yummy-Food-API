using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private ApplicationDBContext _dbContext;
        public AuthRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }
        public async Task<User> SignUp(User user)
        {
            await _dbContext.Users.AddAsync(user); 
            await _dbContext.SaveChangesAsync();
            return user; 
        }
        public async Task<string> GetUserAsync(User user)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (result == null)
            {
                return null;  
            }
            else
            {
                return result.HashedPassword; 
            }
        }
        public async Task<(bool, string)> Login(User user)
        {
            var result = await _dbContext.Users.
                FirstOrDefaultAsync(u => u.Email == user.Email && u.HashedPassword == user.HashedPassword); 
            if (result == null)
            {
                return (false, ""); 
            }
            else
            {
                return (true, result.HashedPassword); 
            }
        }
       
        //public async Task<string> GetRefreshToken()


    }
}
