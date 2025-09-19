using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IRiderService
    {
        Task AcceptOrder(Guid orderId, string userEmail);
        Task<RiderProfile?> AddRiderProfileAsync(RiderProfileDTO riderProfileDTO, string userEmail);
        Task<List<Order>?> GetAllOrdersAsync();
    }
}
