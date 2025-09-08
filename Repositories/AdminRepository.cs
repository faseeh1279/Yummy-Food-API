using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.WebSockets;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;

namespace Yummy_Food_API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminRepository(ApplicationDBContext dbContext, IWebHostEnvironment environment, 
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        

        public async Task<string> AddItemAsync(ItemDTO itemDTO)
        {
            bool itemInDb = await _dbContext.Items.AnyAsync(i => i.Name == itemDTO.Name); 
            if (itemInDb)
            {
                return "Item with the same name already exists"; 
            }
            var itemCategories = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.Category == itemDTO.Category);
            if(itemCategories != null)
            {
                var query = new Item
                {
                    Id = Guid.NewGuid(),
                    Name = itemDTO.Name,
                    Description = itemDTO.Description,
                    Price = itemDTO.Price,
                    Discount = itemDTO.Discount,
                    ItemCategory = itemCategories,
                    ItemCategoryId = itemCategories.ID
                };
                var result = await _dbContext.Items.AddAsync(query);
                await _dbContext.SaveChangesAsync();
                return "Item Added Successfully"; 
            }
            else
            {
                return "Add Category First! Then Add Item"; 
            }
        }

        public async Task<ItemCategory> GetItemCategoryAsync(string Category)
        {
            var result = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.Category == Category);
            return result; 
        }

        public async Task<string> AddItemCategoryAsync(ItemCategory itemCategory)
        {
            bool categoryExists = await _dbContext.ItemCategories.AnyAsync(i => i.Category == itemCategory.Category);
            if (!categoryExists)
            {
                await _dbContext.ItemCategories.AddAsync(itemCategory);
                await _dbContext.SaveChangesAsync();
                return "Category Added Successfully"; 
            }
            else
            {
                return "Category Already Exists"; 
            }
        }

        public async Task<List<ItemCategory>> GetAllCategoriesAsync()
        {
            var result = await _dbContext.ItemCategories.ToListAsync();
            if (result != null)
            {
                return result; 
            }
            else
            {
                return null; 
            }
        }

        public async Task<string> UpdateCategoryAsync(Guid CategoryId, string categoryName)
        {
            var category = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.ID == CategoryId); 
            if (category != null)
            {
                var result = new ItemCategory
                {
                    ID = category.ID,
                    Category = categoryName
                };
                return "Category Updated Successfully"; 
            }
            else
            {
                return "Category Not Available"; 
            }
        }

        public async Task<string> DeleteCategoryAsync(Guid CategoryId)
        {
            var result = await _dbContext.ItemCategories.FindAsync(CategoryId); 
            if(result != null)
            {
                _dbContext.ItemCategories.Remove(result); 
                await _dbContext.SaveChangesAsync();
                return "Item Removed Successfully"; 
            }
            else
            {
                return "Item Not Found"; 
            }
        }

        public async Task<ItemImage> Upload(ItemImage image)
        {
            // Make sure the Images folder exists
            var folderPath = Path.Combine(_environment.ContentRootPath, "Images/Item-Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Add extension to the file name
            var fileNameWithExt = $"{image.FileName}{image.FileExtension}";
            var localFilePath = Path.Combine(folderPath, image.ItemId.ToString());

            // Save the file
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.FormFile.CopyToAsync(stream);

            // Build URL
            var request = _httpContextAccessor.HttpContext.Request;
            var urlFilePath = $"{request.Scheme}://{request.Host}/api/Admin/Get-Item-Image/{image.ItemId}";

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

        public async Task<List<ItemCategory>> GetAllItemCategoriesAsync()
        {
            return await _dbContext.ItemCategories.ToListAsync();
        }

        public async Task<string> DeleteItemAsync(Guid id)
        {
            var item = await _dbContext.Items.FindAsync(id);

            if (item != null)
            {
                _dbContext.Items.Remove(item);
                await _dbContext.SaveChangesAsync(); 
                return "Item Deleted successfully";
            }
            else
                return "Item not Found"; 
        }

        public async Task<string> UpdateItemAsync(Guid Id, ItemDTO itemDTO)
        {
            var item = await _dbContext.Items.FindAsync(Id);
             
            if (item != null)
            {
                var itemCategory = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.Category == itemDTO.Category);
                if(itemCategory != null)
                {
                    item.Name = itemDTO.Name;
                    item.Description = itemDTO.Description;
                    item.Price = itemDTO.Price;
                    item.Discount = itemDTO.Discount;
                    item.ItemCategory = itemCategory!;
                    await _dbContext.SaveChangesAsync();
                    return "Item Updated Successfully"; 
                }
                else
                {
                return "Category Not Exists! Create one to continue"; 

                }
            }
            else
            {
                return "Item Not Found!"; 
            }
        }
    }
}
