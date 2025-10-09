using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ServiceResponse<ItemResponseDTO>> AddItemAsync(ItemDTO itemDTO);
        Task<ServiceResponse<Item>> DeleteItemAsync(Guid id);
        Task<ServiceResponse<ItemDTO>> UpdateItemAsync(Guid id, ItemDTO itemDTO);
        Task<ServiceResponse<ItemCategoryDTO>> AddCategoryAsync(ItemCategoryDTO itemCategoryDTO);
        Task<ServiceResponse<ItemCategory>> DeleteCategoryAsync(Guid CategoryId);
        Task<ServiceResponse<ItemCategoryDTO>> UpdateCategoryAsync(Guid CategoryId, ItemCategoryDTO itemCategoryDTO);
        Task<ServiceResponse<List<ItemCategoryDTO>>> GetAllCategoriesAsync();
        Task<List<Order>> GetAllOrdersAsync();
        Task<ServiceResponse<IEnumerable<ItemWithImageResponseDTO>>> GetAllItemsAsync();
        Task<ServiceResponse<List<ComplaintDTO>>> FetchAllComplaints(); 
        Task<ServiceResponse<ComplaintDTO>> UpdateComplaintStatus(Guid complaintID, ComplaintStatus complaintStatus);
        Task<ServiceResponse<List<UserDTO>>> GetUsersListAsync();
        Task<ServiceResponse<UserDTO>> BlockUserAsync(Guid userID);

    }
}
