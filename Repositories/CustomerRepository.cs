using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
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

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _dbContext.Items.ToListAsync(); 
        }
        public async Task<List<ItemCategory>> GetAllItemCategoriesAsync()
        {
            return await _dbContext.ItemCategories.ToListAsync(); 
        }
        public async Task<List<ItemImage>> GetAllItemImagesAsync()
        {
            return await _dbContext.ItemImages.ToListAsync();
        }
        public async Task<Order> PlaceOrderAsync(Order order)
        {
            var result = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return result.Entity; 
        }

        

    }
}
