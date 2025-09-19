using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<ItemResponseDTO>> GetAllItemsListAsync();
        Task<string> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail, string Address);
        Task<Order?> DeleteOrderAsync(Guid orderId, string userEmail);
    }
}
