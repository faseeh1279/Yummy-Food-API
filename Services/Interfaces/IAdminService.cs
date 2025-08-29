using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<string> AddItemAsync(ItemDTO itemDTO);
        Task<List<Order>> GetAllOrdersAsync();

        Task<List<Complaint>> GetAllComplaintsAsync(); 
    }
}
