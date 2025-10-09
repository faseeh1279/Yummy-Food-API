using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<ItemCategory> AddCategoryAsync(ItemCategory itemCategory);
        Task<Item> AddItemAsync(Item item);
        Task<ItemCategory?> DeleteCategoryAsync(Guid CategoryId);
        Task<Item?> DeleteItemAsync(Guid id);
        Task<List<ItemCategory>?> FetchCategoriesAsync();
        Task<ItemCategory?> FetchCategoryByIdAsync(Guid categoryID);
        Task<Item?> FetchItemByIdAsync(Guid itemID);
        Task<List<Item>?> FetchItemsAsync();
        Task<List<ItemImage>> GetAllItemImagesAsync();
        Task<List<Item>> GetAllItemsAsync();
        Task<List<Order>> GetAllOrdersAsync();
        Task<ItemCategory?> UpdateCategoryAsync(ItemCategory itemCategory);
        Task<Item?> UpdateItemAsync(Guid Id, Item item);
        Task<ItemImage> Upload(ItemImage image);
        Task<List<Order>?> FetchCompletedOrdersAsync();
        Task<List<Complaint>?> FetchAllComplaintsAsync();
        Task<Complaint?> UpdateComplaintStatus(Guid complaintID, ComplaintStatus complaintStatus);
        Task<List<User>?> GetUsersListAsync();
        Task<User?> GetUserAsync(Guid userID);
        Task<User?> UpdateUserAsync(User user);
    }
}
