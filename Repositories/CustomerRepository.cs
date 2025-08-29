using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;

namespace Yummy_Food_API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _dbcontext; 
        public CustomerRepository(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _dbcontext.Items.ToListAsync(); 
        }

        public async Task<Item> GetItemAsync(Guid itemID)
        {
            var result = await _dbcontext.Items.FirstOrDefaultAsync(i => i.Id == itemID);
            return result; 
        }

        public async Task<List<ItemCategory>> GetItemsCategoriesAsync()
        {
            return await _dbcontext.ItemCategories.ToListAsync(); 
        }

        public async Task<ItemCategory> GetItemCategoryAsync(Guid itemCategoryID)
        {
            var result = await _dbcontext.ItemCategories.FirstOrDefaultAsync(i => i.ID == itemCategoryID);
            return result; 
        }

        public async Task<string> PlaceOrderAsync(Order order)
        {
            await _dbcontext.Orders.AddAsync(order);
            await _dbcontext.SaveChangesAsync();
            return "Order placed successfully";
        }

    }
}
