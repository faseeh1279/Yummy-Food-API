using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResponse<IEnumerable<ItemWithImageResponseDTO>>> GetAllItemsListAsync();
        Task<ServiceResponse<OrderDTO>> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail, string Address);
        Task<ServiceResponse<OrderDTO>> DeleteOrderAsync(Guid orderId, string userEmail);
        Task<ServiceResponse<List<OrderDTO>>> GetDeliveredOrderHistory(string userEmail);
        Task<ServiceResponse<OrderDTO>> GetCurrentPendingOrderAsync(string userEmail); 
    }
}
