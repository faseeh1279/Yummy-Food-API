using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<List<ItemCategory>> GetAllCategoriesAsync();
        Task<List<ItemImage>> GetAllItemImagesAsync();
        Task<List<Order>> GetAllOrdersAsync();
        Task<User?> GetUserAsync(string userEmail);
        Task<Order?> DeleteOrderAsync(Guid orderId);
        Task<CustomerProfile?> GetCustomerProfileAsync(Guid userId);
        Task<CustomerProfile> AddCustomerProfileAsync(CustomerProfile customerProfile);
        Task<Order> PlaceOrderAsync(Order order);
        Task AddOrderItemsRangeAsync(List<OrderItem> items);
        Task UpdateOrderAsync(Order order);
        Task<Order> GetPendingOrderByCustomerAsync(Guid customerProfileId);
        Task<List<Order>> GetOrderHistoryAsync(Guid customerProfileId);
    }
}
