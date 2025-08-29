using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Yummy_Food_API.Models.Domain;
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
        

        public async Task<string> AddItemAsync(Item item)
        {
            await _dbContext.Items.AddAsync(item); 
            await _dbContext.SaveChangesAsync();
            return "Item added successfully";
        }

        public async Task<ItemCategory> GetItemCategoryAsync(string Category)
        {
            var result = await _dbContext.ItemCategories.FirstOrDefaultAsync(i => i.Category == Category);
            return result; 
        }

        public async Task AddItemCategoryAsync(ItemCategory itemCategory)
        {
            await _dbContext.ItemCategories.AddAsync(itemCategory); 
            await _dbContext.SaveChangesAsync();
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
            var localFilePath = Path.Combine(folderPath, fileNameWithExt);

            // Save the file
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.FormFile.CopyToAsync(stream);

            // Build URL
            var request = _httpContextAccessor.HttpContext.Request;
            var urlFilePath = $"{request.Scheme}://{request.Host}/Images/{fileNameWithExt}";

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
    }
}
