using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IRiderService
    {
        Task<ServiceResponse<OrderDTO>> AcceptOrder(Guid orderId, string userEmail);
        Task<ServiceResponse<RiderProfile>> AddRiderProfileAsync(RiderProfileDTO riderProfileDTO, string userEmail);
        Task<ServiceResponse<OrderDTO>> CompleteOrderAsync(Guid orderId, string userEmail);
        Task<ServiceResponse<List<OrderDTO>>> GetAllOrdersAsync(string userEmail);
        Task<ServiceResponse<OrderDTO>> DeleteOrderAsync(string userEmail);
        Task<ServiceResponse<OrderDTO>> GetAcceptedOrderDetail(string userEmail);
        Task<ServiceResponse<List<OrderDTO>>> GetCompletedOrders(string userEmail);
    }
}
