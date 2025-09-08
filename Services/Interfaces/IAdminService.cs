using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<string> AddItemAsync(ItemDTO itemDTO);
        Task<string> AddCategoryAsync(ItemCategoryDTO itemCategoryDTO);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Complaint>> GetAllComplaintsAsync();
        Task<IEnumerable<ItemResponseDTO>> GetAllItemsAsync();
        Task<string> DeleteItemAsync(Guid id);
        Task<string> UpdateItemAsync(Guid id, ItemDTO itemDTO);
        Task<string> DeleteCategoryAsync(Guid CategoryId);
        Task<string> UpdateCategoryAsync(Guid CategoryId, ItemCategoryDTO itemCategoryDTO);
        Task<List<object>> GetAllCategoriesAsync();
    }
}
