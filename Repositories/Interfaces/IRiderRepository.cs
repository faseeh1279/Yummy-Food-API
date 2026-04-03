using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IRiderRepository
    {
        Task<User> GetUserAsync(string userEmail);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderAsync(Order order);
        Task<RiderProfile?> GetRiderProfileAsync(Guid userId); 
        Task DeclineOrderAsync(Order order);
        Task<Order?> FetchPendingOrdersAsync(Guid riderProfileID);
        Task<RiderProfile?> CreateRiderProfileAsync(RiderProfile riderProfile);
        Task<RiderProfile?> UpdateRiderProfileAsync(RiderProfile riderProfile);
        Task<Order?> FetchAcceptedOrderAsync(Guid riderProfileID);
        Task<RiderProfile?> DeleteRiderProfileAsync(Guid riderProfileId);
        Task<List<Order>?> GetCompletedOrdersAsync(Guid riderProfileId);
    }
}
