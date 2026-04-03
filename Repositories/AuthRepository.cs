using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDBContext _dbContext;
    private readonly IUsersRepository _usersRepository; 
    public AuthRepository(ApplicationDBContext context, IUsersRepository usersRepository)
    {
        _dbContext = context;
        _usersRepository = usersRepository;
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
    public async Task<User?> Login(User user)
    {
        var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.HashedPassword == user.HashedPassword);
        if (result != null)
            return result;
        return null; 
    }
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
    public async Task<RefreshToken?> UpdateRefreshTokenAsync(string refreshToken)
    {
        var tokenExists = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
        if (tokenExists == null)
            return null; 

        tokenExists = new RefreshToken
        {
            Id = tokenExists.Id,
            UserId = tokenExists.UserId,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        _dbContext.RefreshTokens.Update(tokenExists); 
        await _dbContext.SaveChangesAsync();

        return tokenExists; 
    }
    public async Task<RefreshToken?> GenerateRefreshToken(string Token)
    {
        var result = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == Token);
        if (result != null)
            return result; 
        return null;
    }
}
