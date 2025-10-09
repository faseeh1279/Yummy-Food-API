using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Repositories.Interfaces;

namespace Yummy_Food_API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _dbContext; 
        public CustomerRepository(ApplicationDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<List<Item>?> GetAllItemsAsync()
        {
            var result = await _dbContext.Items.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }
        public async Task<List<ItemCategory>?> GetAllCategoriesAsync()
        {
            var result = await _dbContext.ItemCategories.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }
        public async Task<List<ItemImage>> GetAllItemImagesAsync()
        {
            return await _dbContext.ItemImages.ToListAsync();
        }
        public async Task<List<Order>?> GetAllOrdersAsync()
        {
            var result = await _dbContext.Orders.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }
        public async Task<User?> GetUserAsync(string userEmail)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (result!=null)
                return result;
            return null; 
        } 
        public async Task<Order?> DeleteOrderAsync(Guid orderId)
        {
            var result = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId); 
            if(result != null)
            {
                _dbContext.Orders.Remove(result);
                await _dbContext.SaveChangesAsync();
                return result; 
            }
            return null; 
        }
        public async Task<Order> PlaceOrderAsync(Order order)
        {
            var result = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return result.Entity; 
        }
        public async Task<CustomerProfile?> GetCustomerProfileAsync(Guid userId)
        {
            var result = await _dbContext.CustomerProfiles.FirstOrDefaultAsync(cp => cp.UserId == userId);
            if (result != null)
                return result;
            return null; 
        }
        public async Task AddOrderItemsRangeAsync(List<OrderItem> items)
        {
            await _dbContext.OrderItems.AddRangeAsync(items); 
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateOrderAsync(Order order)
        {
           _dbContext.Orders.Update(order);
           await _dbContext.SaveChangesAsync();
        }
        public async Task<CustomerProfile> AddCustomerProfileAsync(CustomerProfile customerProfile)
        {
            var result = await _dbContext.CustomerProfiles.AddAsync(customerProfile);
            return result.Entity; 
        }
        public async Task<Order?> GetPendingOrderByCustomerProfileAsync(Guid customerProfileId)
        {
            var result = await _dbContext.Orders.FirstOrDefaultAsync(o => o.CustomerProfileId == customerProfileId);
            if (result != null)
                return result; 
            return null; 
        }
        public async Task<List<Order>?> GetDeliveredOrderHistoryAsync(Guid customerProfileId)
        {
            var result = await _dbContext.Orders.Where(o => o.CustomerProfileId == customerProfileId && o.OrderStatus == Enums.OrderStatus.Delivered).ToListAsync();
            if (result != null)
                return result; 
            return null; 
        }

    }
}
