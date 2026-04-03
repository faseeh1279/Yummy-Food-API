using Yummy_Food_API.Models.Domain; 
namespace Yummy_Food_API.Repositories.Interfaces;

public interface ICategoriesRepository
{
    Task<ItemCategory> AddCategoryAsync(ItemCategory itemCategory);
    Task<ItemCategory?> DeleteCategoryAsync(Guid CategoryId);
    Task<List<ItemCategory>?> FetchCategoriesAsync();
    Task<ItemCategory?> FetchCategoryByIdAsync(Guid categoryID);
    Task<ItemCategory?> UpdateCategoryAsync(ItemCategory itemCategory);
}
