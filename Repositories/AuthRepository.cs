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
        private readonly ApplicationDBContext _dbContext;
        public AuthRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<List<User>?> GetUsersAsync()
        {
            var result = await _dbContext.Users.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }
        public async Task<User?> SignUp(User user)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
            if (userExists)
                return null;
            await _dbContext.Users.AddAsync(user); 
            await _dbContext.SaveChangesAsync();
            return user; 
        }
        //public async Task<string> SignUp(User user)
        //{
        //    var userExists = await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
        //    if (userExists)
        //        return $"User with this email {user.Email} already exists."; 
        //    await _dbContext.Users.AddAsync(user);
        //    await _dbContext.SaveChangesAsync();
        //    return "User Added Successfully";
        //}
        public async Task<User?> GetUserAsync(string userEmail)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (result != null)
                return result;
            return null; 
        }

        //public async Task<(bool, string, Guid)> Login(User user)
        //{
        //    var result = await _dbContext.Users.
        //        FirstOrDefaultAsync(u => u.Email == user.Email && u.HashedPassword == user.HashedPassword);
        //    if (result == null)
        //    {
        //        return (false, "", Guid.Empty);
        //    }
        //    else
        //    {
        //        return (true, result.HashedPassword, result.Id);
        //    }
        //}

        public async Task<User?> Login(User user)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.HashedPassword == user.HashedPassword);
            if (result != null)
                return result;
            return null; 
        }

        //public async Task AddRefreshToken(RefreshToken refreshToken)
        //{
        //    var tokenExists = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == refreshToken.UserId);
        //    if (tokenExists != null)
        //    {
        //        var tokenToUpdate = new RefreshToken
        //        {
        //            UserId = refreshToken.UserId,
        //            Token = refreshToken.Token,
        //            Expires = DateTime.UtcNow.AddDays(7)
        //        };
        //        await UpdateRefreshToken(tokenToUpdate);
        //    }
        //    else
        //    {
        //        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        //        await _dbContext.SaveChangesAsync();

        //    }
        //}

        public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
        {
            var tokenExists = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == refreshToken.UserId); 
            if (tokenExists != null)
            {
                _dbContext.RefreshTokens.Update(refreshToken);
                await _dbContext.SaveChangesAsync();
                return refreshToken; 
            }
            else
            {
                await _dbContext.RefreshTokens.AddAsync(refreshToken); 
                await _dbContext.SaveChangesAsync();
                return refreshToken; 
            }
        }

        public async Task<RefreshToken?> GetRefreshToken(string Token)
        {
            var result = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == Token);
            if (result != null)
                return result; 
            return null;
        }

        //public async Task<User?> GetUserDataByRefreshToken(string refreshToken)
        //{
        //    var result = await _dbContext.Users.FirstOrDefaultAsync(t => t.RefreshToken.Token == refreshToken);
        //    if (result != null)
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public async Task UpdateRefreshToken(RefreshToken refreshToken)
        //{
        //    _dbContext.RefreshTokens.Update(refreshToken);
        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
