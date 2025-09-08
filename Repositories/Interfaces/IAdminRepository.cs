using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        // Items
        Task<string> AddItemAsync(ItemDTO itemDTO);
        Task<ItemCategory> GetItemCategoryAsync(string Category);
        Task<string> AddItemCategoryAsync(ItemCategory itemCategory);
        Task<List<ItemCategory>> GetAllCategoriesAsync();
        Task<ItemImage> Upload(ItemImage itemImage);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Complaint>> GetAllComplaintsAsync();
        Task<List<Item>> GetAllItemsAsync();
        Task<List<ItemImage>> GetAllItemImagesAsync(); 
        Task<List<ItemCategory>> GetAllItemCategoriesAsync();
        Task<string> DeleteItemAsync(Guid id);
        Task<string> UpdateItemAsync(Guid Id, ItemDTO itemDTO);
        Task<string> UpdateCategoryAsync(Guid CategoryId, string categoryName);
        Task<string> DeleteCategoryAsync(Guid CategoryId);
    }
}
