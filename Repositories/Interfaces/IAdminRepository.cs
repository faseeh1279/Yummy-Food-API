using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        
        Task<string> AddItemAsync(Item item);
        Task<ItemCategory> GetItemCategoryAsync(string Category);
        Task AddItemCategoryAsync(ItemCategory itemCategory);
        Task<ItemImage> Upload(ItemImage itemImage);
        Task<List<Order>> GetAllOrdersAsync();

        Task<List<Complaint>> GetAllComplaintsAsync(); 
    }
}
