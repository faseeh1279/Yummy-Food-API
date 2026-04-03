using Microsoft.EntityFrameworkCore;
using Yummy_Food_API;
using Yummy_Food_API.Models.Domain;
namespace Yummy_Food_API.Repositories;
public class ItemsRepository
{
    private readonly ApplicationDBContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor; 
    public ItemsRepository(ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Item> AddItemAsync(Item item)
    {
        var result = await _dbContext.Items.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Item?> DeleteItemAsync(Guid id)
    {
        var result = await _dbContext.Items.FindAsync(id);

        if (result != null)
        {
            _dbContext.Items.Remove(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        return null;
    }

    public async Task<Item?> UpdateItemAsync(Guid Id, Item item)
    {
        var result = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == Id);

        if (result != null)
        {
            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
        return null;
    }

    public async Task<List<Item>?> FetchItemsAsync()
    {
        var result = await _dbContext.Items.ToListAsync();
        if (result != null)
            return result;
        return null;
    }
    public async Task<Item?> FetchItemByIdAsync(Guid itemID)
    {
        var result = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == itemID);
        if (result != null)
            return result;
        return null;
    }

    #region Item Images 
    public async Task<ItemImage> Upload(ItemImage image)
    {
        // Make sure the Images folder exists
        var folderPath = Path.Combine(_environment.ContentRootPath, "Images/Item-Images");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Add extension to the file name
        var fileNameWithExt = $"{image.Id}{image.FileExtension}";
        var localFilePath = Path.Combine(folderPath, fileNameWithExt);

        // Save the file
        using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.FormFile.CopyToAsync(stream);

        // Build URL
        var request = _httpContextAccessor.HttpContext.Request;
        var urlFilePath = $"{request.Scheme}://{request.Host}/api/Admin/Get-Item-Image/{image.Id}";

        image.FilePath = urlFilePath;

        // Save metadata in DB
        await _dbContext.ItemImages.AddAsync(image);
        await _dbContext.SaveChangesAsync();

        return image;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<List<Complaint>> GetAllComplaintsAsync()
    {
        return await _dbContext.Complaints.ToListAsync();
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _dbContext.Items.ToListAsync();
    }

    public async Task<List<ItemImage>> GetAllItemImagesAsync()
    {
        return await _dbContext.ItemImages.ToListAsync();
    }

    #endregion Item Images 
}
