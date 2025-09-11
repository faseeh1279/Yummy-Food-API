using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<List<ItemCategory>> GetAllItemCategoriesAsync();
        Task<string> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail);
        Task<List<ItemImage>> GetAllItemImagesAsync();
        Task<List<Order>> GetAllOrdersAsync();
    }
}
