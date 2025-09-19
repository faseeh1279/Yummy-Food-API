using Microsoft.EntityFrameworkCore;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Repositories.Interfaces;

namespace Yummy_Food_API.Repositories
{
    public class RiderRepository : IRiderRepository
    {
        private readonly ApplicationDBContext _dbContext; 
        public RiderRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUserAsync(string userEmail)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return result; 
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var result = await _dbContext.Orders.ToListAsync();
            return result; 
        } 
        public async Task AcceptOrderAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync(); 
        }
        public async Task<RiderProfile?> CreateRiderProfileAsync(RiderProfile riderProfile)
        {
            var riderProfileExists = await _dbContext.RiderProfiles.FirstOrDefaultAsync(r => r.UserId == riderProfile.UserId); 
            if(riderProfileExists == null)
            {
                var result = await _dbContext.RiderProfiles.AddAsync(riderProfile);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null; 
        }
        public async Task<RiderProfile?> UpdateRiderProfileAsync(RiderProfile riderProfile)
        {
            var result = await _dbContext.RiderProfiles.FirstOrDefaultAsync(r => r.Id == riderProfile.Id); 
            if (result != null)
            {
                _dbContext.RiderProfiles.Update(riderProfile);
                await _dbContext.SaveChangesAsync();
            }
            return result; 
        }
        public async Task<RiderProfile?> DeleteRiderProfileAsync(Guid riderProfileId)
        {
            var result = await _dbContext.RiderProfiles.FirstOrDefaultAsync(r => r.Id == riderProfileId);
            if (result != null)
            {
                _dbContext.RiderProfiles.Remove(result);
                await _dbContext.SaveChangesAsync();
                return result; 
            }
            return result; 
        }
        public async Task DeclineOrderAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync(); 
        }
        public async Task<List<Order>?> GetCompletedOrdersAsync(Guid riderProfileId)
        {
            var result = await _dbContext.Orders.Where(o => o.RiderProfileId == riderProfileId &&
            o.OrderStatus == Enums.OrderStatus.Delivered).ToListAsync();
            if (result.Count > 0)
            {
                return result; 
            }
            return null; 
        }

    }
}
