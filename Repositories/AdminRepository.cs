using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.WebSockets;
using Yummy_Food_API.Enums;
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

        #region Items 

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

        #endregion Items 

        #region Categories 

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

        #endregion Categories 

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

        #region Orders 
        public async Task<List<Order>?> FetchCompletedOrdersAsync()
        {
            var result =  await _dbContext.Orders.Where(o => o.OrderStatus == Enums.OrderStatus.Delivered).ToListAsync();
            if (result != null)
                return result;
            return null; 
        }


        #endregion Orders

        #region Complaints
        public async Task<List<Complaint>?> FetchAllComplaintsAsync()
        {
            var result = await _dbContext.Complaints.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }

        public async Task<Complaint?> UpdateComplaintStatus(Guid complaintID, ComplaintStatus complaintStatus)
        {
            var result = await _dbContext.Complaints.FirstOrDefaultAsync(c => c.Id == complaintID); 
            if (result!= null)
            {
                result.ComplaintStatus = complaintStatus;
                result.UpdatedAt = DateTime.UtcNow;
                _dbContext.Complaints.Update(result);
                await _dbContext.SaveChangesAsync(); 
            }
            return null; 
        }
        #endregion Complaints

        public async Task<List<User>?> GetUsersListAsync()
        {
            var result = await _dbContext.Users.ToListAsync();
            if (result != null)
                return result;
            return null; 
        }
        public async Task<User?> GetUserAsync(Guid userID)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userID);
            if (result != null)
                return result;
            return null; 
        }
        public async Task<User?> UpdateUserAsync(User user)
        {
            var userExists = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userExists != null)
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            return null; 
        }
    }
}
