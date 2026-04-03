using Microsoft.EntityFrameworkCore;
using Yummy_Food_API;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories;
public class UsersRepository
{
    private readonly ApplicationDBContext _dbContext;
    public UsersRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<List<User>?> GetUsersListAsync()
    {
        var result = await _dbContext.Users.ToListAsync();
        if (result != null)
            return result;
        return null;
    }
    public async Task<User?> GetUserAsync(Guid userID)
    {
        var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userID);
        if (result != null)
            return result;
        return null;
    }
    public async Task<User?> UpdateUserAsync(User user)
    {
        var userExists = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (userExists != null)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        return null;
    }
}