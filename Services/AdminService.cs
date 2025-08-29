using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Web.Http.ModelBinding;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminService(IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment)
        {
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<string> AddItemAsync(ItemDTO itemDTO)
        {
            // Check if item category exists 
            var userItemCategory = await _adminRepository.GetItemCategoryAsync(itemDTO.Category);  
            if (userItemCategory == null)
            {
                var itemCategory = new ItemCategory
                {
                    ID = Guid.NewGuid(),
                    Category = itemDTO.Category, 
                };
                await _adminRepository.AddItemCategoryAsync(itemCategory);
                var item = new Item
                {
                    Id = Guid.NewGuid(),
                    Name = itemDTO.Name,
                    Description = itemDTO.Description,
                    Price = itemDTO.Price,
                    Discount = itemDTO.Discount,
                    ItemCategoryId = itemCategory.ID
                };
                await _adminRepository.AddItemAsync(item);
                return "Item Added Successfully"; 
            }
            else
            {
                Item item = new Item
                {
                    Id = Guid.NewGuid(),
                    Name = itemDTO.Name,
                    Description = itemDTO.Description,
                    Price = itemDTO.Price,
                    Discount = itemDTO.Discount,
                    ItemCategoryId = userItemCategory.ID
                };
                await _adminRepository.AddItemAsync(item); 
                return "Category already exists, item added without category";
            }
             
        }

        
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _adminRepository.GetAllOrdersAsync();
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await _adminRepository.GetAllComplaintsAsync();
        }
    }
}
