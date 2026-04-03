using Microsoft.EntityFrameworkCore;
using Yummy_Food_API.Repositories.Interfaces; 
using Yummy_Food_API;

namespace Yummy_Food_API.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
private readonly ApplicationDBContext _dbContext; 
public CategoriesRepository(ApplicationDBContext dbContext)
{
    _dbContext = dbContext; 
}


public async Task<ItemCategory> AddCategoryAsync(ItemCategory itemCategory)
{
    var result = await _dbContext.ItemCategories.AddAsync(itemCategory);
    return result.Entity;
}

public async Task<ItemCategory?> UpdateCategoryAsync(ItemCategory itemCategory)
{
    var result = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.ID == itemCategory.ID);
    if (result != null)
    {
        _dbContext.ItemCategories.Update(itemCategory);
        await _dbContext.SaveChangesAsync();
        var item = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.ID == itemCategory.ID);
        return item;
    }
    return null;
}

public async Task<ItemCategory?> DeleteCategoryAsync(Guid CategoryId)
{
    var result = await _dbContext.ItemCategories.FindAsync(CategoryId);
    if (result != null)
    {
        _dbContext.ItemCategories.Remove(result);
        await _dbContext.SaveChangesAsync();
        return result;
    }
    return null;

}

public async Task<List<ItemCategory>?> FetchCategoriesAsync()
{
    var result = await _dbContext.ItemCategories.ToListAsync();
    if (result != null)
        return result;
    return null;
}

public async Task<ItemCategory?> FetchCategoryByIdAsync(Guid categoryID)
{
    var result = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.ID == categoryID);
    if (result != null)
        return result;
    return null;
}

}