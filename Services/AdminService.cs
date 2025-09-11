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
            var result = await _adminRepository.AddItemAsync(itemDTO);
            return result; 
        }

        public async Task<string> AddCategoryAsync(ItemCategoryDTO itemCategoryDTO)
        {
            var data = new ItemCategory
            {
                ID = Guid.NewGuid(),
                Category = itemCategoryDTO.Category
            };
            var result = await _adminRepository.AddItemCategoryAsync(data);
            return result; 
        }

        public async Task<string> DeleteCategoryAsync(Guid CategoryId)
        {
            var result = await _adminRepository.DeleteCategoryAsync(CategoryId);
            return result; 
        }

        public async Task<string> UpdateCategoryAsync(Guid CategoryId, ItemCategoryDTO itemCategoryDTO)
        {
            
            var result = await _adminRepository.UpdateCategoryAsync(CategoryId, itemCategoryDTO.Category);
            return result; 
        }

        public async Task<List<object>> GetAllCategoriesAsync()
        {
            var data = await _adminRepository.GetAllCategoriesAsync();
            var result = new List<object>(); 
            foreach(var item in data)
            {
                result.Add(new
                {
                    ID = item.ID,
                    Category = item.Category
                });  
            }
            return result; 
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _adminRepository.GetAllOrdersAsync();
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await _adminRepository.GetAllComplaintsAsync();
        }

        public async Task<IEnumerable<ItemResponseDTO>> GetAllItemsAsync()
        {
            var items = await _adminRepository.GetAllItemsAsync();
            var itemImages = await _adminRepository.GetAllItemImagesAsync();
             var itemCategories = await _adminRepository.GetAllItemCategoriesAsync();

            var result = ConvertItemAndImagesToResponse(items, itemImages, itemCategories);
            return result;
        }

        public async Task<string> DeleteItemAsync(Guid id)
        {
            var result = await _adminRepository.DeleteItemAsync(id);
            return result; 
        }

        public async Task<string> UpdateItemAsync(Guid id, ItemDTO itemDTO)
        {
            var result = await _adminRepository.UpdateItemAsync(id, itemDTO);
            return result;
        }

        private ItemResponseDTO BindItemWithReleventImages(Item item, List<ItemImage> itemImages)
        {
            var images = new List<ItemImageResponseDTO>(); 
            foreach(var image in itemImages)
            {
                if (image != null && image.Id == item.Id)
                {
                    images.Add(new ItemImageResponseDTO
                    {
                        ImageId = image.Id,
                        FilePath = image.FilePath,
                    });
                }
            }

           
                var result = new ItemResponseDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Discount = item.Discount,
                    Images = images
                };
                return result;
               
        }
        private IEnumerable<ItemResponseDTO> ConvertItemAndImagesToResponse(List<Item> items, List<ItemImage> itemImages, List<ItemCategory> itemCategories)
        {
            var itemResponses = new List<ItemResponseDTO>();
            
            foreach (var item in items)
            {
                var imagesForItem = itemImages
                    .Where(img => img.ItemId == item.Id)
                    .Select(img => new ItemImageResponseDTO
                    {
                        ImageId = img.Id,
                        FilePath = img.FilePath
                    })
                    .ToList();
                if(item.Discount == 0)
                {
                    itemResponses.Add(new ItemResponseDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Description = item.Description,
                        Discount = item.Discount,
                        FinalPrieWithDiscount = item.Price,
                        Category = item.ItemCategory.Category, // TODO: fetch from itemCategories
                        Images = imagesForItem
                    });
                }
                else
                {
                    itemResponses.Add(new ItemResponseDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Description = item.Description,
                        Discount = item.Discount, 
                        FinalPrieWithDiscount = item.Price - (item.Price * item.Discount / 100),
                        Category = item.ItemCategory.Category, // TODO: fetch from itemCategories
                        Images = imagesForItem
                    });
                }
            }

            return itemResponses;
        }
    }
}
