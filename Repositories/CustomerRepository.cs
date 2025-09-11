using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Yummy_Food_API.Enums;
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
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var result = await _dbContext.Orders.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }
        public async Task<string> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            var customerProfile = await _dbContext.CustomerProfiles.FirstOrDefaultAsync(u => u.UserId == user!.Id); 
            if(customerProfile == null)
            {
                var cProfile = new CustomerProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = user!.Id,
                }; 
                await _dbContext.CustomerProfiles.AddAsync(cProfile);
                await _dbContext.SaveChangesAsync(); 
            }

            var orderExists = await _dbContext.Orders.AnyAsync(
                o => o.CustomerProfileId == user!.Id
            && o.OrderStatus == Enums.OrderStatus.Pending
            ); 
          
            if (orderExists)
            {
                return "You have a pending Order"; 
            }
            else
            {
               
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    CompletedAt = DateTime.MinValue,
                    OrderStatus = Enums.OrderStatus.Pending,
                    CustomerProfileId = customerProfile!.Id,
                    RiderProfileId = null,
                };
                decimal price = 0;
                var orderItems = new List<OrderItem>(); 
                foreach (var o in orderItemsDTOs)
                {
                    price += o.UnitPrice * o.Quantity;
                    orderItems.Add(new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id, 
                        ItemId = o.ItemId, 
                        UnitPrice = o.UnitPrice, 
                        Quantity = o.Quantity,
                    }); 
                }
               
                order.TotalPrice = price;
                await _dbContext.OrderItems.AddRangeAsync(orderItems);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Orders.AddAsync(order); 
                await _dbContext.SaveChangesAsync();
                return "Order Placed."; 
            }
        }
    }
}
