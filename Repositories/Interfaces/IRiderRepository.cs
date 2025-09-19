using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IRiderRepository
    {
        Task<User> GetUserAsync(string userEmail);
        Task<List<Order>> GetAllOrdersAsync();
        Task AcceptOrderAsync(Order order);
        Task DeclineOrderAsync(Order order);
        Task<RiderProfile?> CreateRiderProfileAsync(RiderProfile riderProfile);
        Task<RiderProfile?> UpdateRiderProfileAsync(RiderProfile riderProfile);
        Task<RiderProfile?> DeleteRiderProfileAsync(Guid riderProfileId);
        Task<List<Order>?> GetCompletedOrdersAsync(Guid riderProfileId);
    }
}
