using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web.Http.ModelBinding;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Mappings;
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
        private readonly IMapper _mapper;
        public AdminService(IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ItemResponseDTO>> AddItemAsync(ItemDTO itemDTO)
        {
            var response = new ServiceResponse<ItemResponseDTO>();
            var items = await _adminRepository.GetAllItemsAsync();
            var categories = await _adminRepository.FetchCategoriesAsync();

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDTO.Name,
                Description = itemDTO.Description,
                Discount = itemDTO.Discount,
                Price = itemDTO.Price,
                Updated = DateTime.UtcNow,
                ItemCategoryId = categories.FirstOrDefault(c => c.Category == itemDTO.Category)!.ID
            };
            if (categories == null)
            {
                response.Success = false;
                response.Message = "No Categories found, Please add category first";
                return response;
            }

            if (items == null)
            {
                var result = await _adminRepository.AddItemAsync(item);
                response.Success = true;
                var finalResult = _mapper.Map<ItemResponseDTO>(result);
                response.Data = finalResult;
                return response;
            }
            else
            {
                if (items.FirstOrDefault(i => i.Name == itemDTO.Name) != null)
                {
                    response.Success = false;
                    response.Message = "Item with this name Already exists";
                    return response;
                }
                else
                {
                    var result = await _adminRepository.AddItemAsync(item);
                    response.Success = true;
                    var finalResult = _mapper.Map<ItemResponseDTO>(result);
                    response.Data = finalResult;
                    return response;
                }
            }

        }

        public async Task<ServiceResponse<Item>> DeleteItemAsync(Guid id)
        {
            var response = new ServiceResponse<Item>();
            var result = await _adminRepository.DeleteItemAsync(id);
            if (result != null)
            {
                response.Success = true;
                response.Data = result;
                return response;
            }
            response.Success = false;
            response.Message = "Item with this ID not found";
            return response;
        }

        public async Task<ServiceResponse<ItemDTO>> UpdateItemAsync(Guid id, ItemDTO itemDTO)
        {
            var response = new ServiceResponse<ItemDTO>();
            var categories = await _adminRepository.FetchCategoriesAsync();

            if (categories != null && categories.FirstOrDefault(c => c.Category == itemDTO.Category) != null)
            {
                var itemCategory = categories.FirstOrDefault(c => c.Category == itemDTO.Category)!;
                var updatedItem = new Item
                {
                    Id = id,
                    Name = itemDTO.Name,
                    Price = itemDTO.Price,
                    Description = itemDTO.Description,
                    Updated = DateTime.UtcNow,
                    Discount = itemDTO.Discount,
                    ItemCategoryId = itemCategory.ID
                };
                response.Success = true;
                var result = await _adminRepository.UpdateItemAsync(id, updatedItem);
                if (result != null)
                {
                    var finalResponse = new ItemDTO
                    {
                        Name = updatedItem.Name,
                        Description = updatedItem.Description,
                        Price = updatedItem.Price,
                        Discount = updatedItem.Discount,
                        Category = itemCategory.Category
                    };
                    response.Data = finalResponse;
                    response.Success = true;
                    return response;
                }
                else
                {
                    response.Message = "Item with this ID not found";
                    response.Success = false;
                    return response;
                }

            }
            else
            {
                response.Success = false;
                response.Message = "No Categories found, Please add category first";
                return response;
            }
        }

        public async Task<ServiceResponse<ItemCategoryDTO>> AddCategoryAsync(ItemCategoryDTO itemCategoryDTO)
        {
            var response = new ServiceResponse<ItemCategoryDTO>();
            var data = new ItemCategory
            {
                ID = Guid.NewGuid(),
                Category = itemCategoryDTO.Category
            };
            var result = await _adminRepository.AddCategoryAsync(data);
            var finalResult = new ItemCategoryDTO
            {
                Category = result.Category
            };  
            response.Data = finalResult;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<ItemCategory>> DeleteCategoryAsync(Guid CategoryId)
        {
            var response = new ServiceResponse<ItemCategory>();
            var result = await _adminRepository.DeleteCategoryAsync(CategoryId);
            if (result != null)
            {
                response.Success = true;
                response.Data = result;
                return response;
            }
            response.Success = false;
            response.Message = "Category with this ID not found";
            return response;
        }

        public async Task<ServiceResponse<ItemCategoryDTO>> UpdateCategoryAsync(Guid CategoryId, ItemCategoryDTO itemCategoryDTO)
        {
            var response = new ServiceResponse<ItemCategoryDTO>();
            var itemCategory = new ItemCategory
            {
                ID = CategoryId,
                Category = itemCategoryDTO.Category
            };
            var result = await _adminRepository.UpdateCategoryAsync(itemCategory);
            if (result != null)
            {
                var finalResult = new ItemCategoryDTO
                {
                    Category = result.Category
                };
                response.Success = true;
                response.Data = finalResult;
                return response;
            }
            response.Success = false;
            response.Message = "Category with this ID not found";
            return response;
        }

        public async Task<ServiceResponse<List<ItemCategoryDTO>>> GetAllCategoriesAsync()
        {
            var response = new ServiceResponse<List<ItemCategoryDTO>>();
            var result = await _adminRepository.FetchCategoriesAsync();
            if (result != null)
            {
                var finalResult = new List<ItemCategoryDTO>();
                foreach (var category in result)
                {
                    finalResult.Add(new ItemCategoryDTO
                    {
                        Category = category.Category
                    }); 
                }
                response.Success = true;
                response.Data = finalResult;
                return response;
            }
            response.Success = false;
            response.Message = "No Categories found";
            return response;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _adminRepository.GetAllOrdersAsync();
        }

        public async Task<ServiceResponse<List<Complaint>>> GetAllComplaintsAsync()
        {
            var response = new ServiceResponse<List<Complaint>>();
            var result = await _adminRepository.FetchAllComplaintsAsync();
            if (result != null)
            {
                response.Success = true;
                response.Data = result;
                return response; 
            }
            else
            {
                response.Success = false;
                response.Message = "No Complaints found";
                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ItemWithImageResponseDTO>>> GetAllItemsAsync()
        {
            var response = new ServiceResponse<IEnumerable<ItemWithImageResponseDTO>>();
            var items = await _adminRepository.GetAllItemsAsync();
            var itemImages = await _adminRepository.GetAllItemImagesAsync();
            var itemCategories = await _adminRepository.FetchCategoriesAsync();
            if (items == null || itemImages == null || itemCategories == null)
            {
                response.Success = false;
                response.Message = "No Items found";
                return response;
            }
            else
            {
                var result = ConvertItemAndImagesToResponse(items, itemImages, itemCategories);
                response.Success = true;
                response.Data = result;
                return response; 

            }
        }

        public async Task<ServiceResponse<ComplaintDTO>> UpdateComplaintStatus(Guid complaintID, ComplaintStatus complaintStatus)
        {
            var response = new ServiceResponse<ComplaintDTO>();
            var result = await _adminRepository.UpdateComplaintStatus(complaintID, complaintStatus); 
            if (result!= null)
            {
                var finalResponse = new ComplaintDTO
                {
                    ComplaintName = result.ComplaintName, 
                    ComplaintDescription = result.ComplaintDescription, 
                    ComplaintStatus = result.ComplaintStatus
                }; 
                response.Success = true;
                response.Data = finalResponse;
                return response; 
                
            }
            response.Success = false;
            response.Message = "Compalint not found";
            return response; 
        }

        public async Task<ServiceResponse<List<ComplaintDTO>>> FetchAllComplaints()
        {
            var response = new ServiceResponse<List<ComplaintDTO>>(); 
            var result = await _adminRepository.FetchAllComplaintsAsync();
            if (result!= null)
            {
                var finalResult = new List<ComplaintDTO>(); 
                foreach(var complaint in result)
                {
                    finalResult.Add(new ComplaintDTO
                    {
                        ComplaintName = complaint.ComplaintName,
                        ComplaintDescription = complaint.ComplaintDescription, 
                        ComplaintStatus = complaint.ComplaintStatus
                    }); 
                }
                response.Success = true;
                response.Data = finalResult;
                return response; 
                
            }
            response.Success = false;
            response.Message = "No Complaints Found";
            return response; 
        }

        public async Task<ServiceResponse<List<UserDTO>>> GetUsersListAsync()
        {
            var response = new ServiceResponse<List<UserDTO>>(); 
            var result = await _adminRepository.GetUsersListAsync(); 
            if (result == null)
            {
                response.Success = false;
                response.Message = "No Users Available";
                return response; 
            }
            var users = new List<UserDTO>(); 
            foreach(var user in result)
            {
                users.Add(new UserDTO
                {
                    Username = user.Username, 
                    Email = user.Email, 
                    PhoneNumber = user.PhoneNumber, 
                    isBlocked = user.isBlocked, 
                    Id = user.Id, 
                    Role = user.Role 
                }); 
            }
            response.Success = true;
            response.Data = users;
            return response; 

        }

        public async Task<ServiceResponse<UserDTO>> BlockUserAsync(Guid userID)
        {
            var response = new ServiceResponse<UserDTO>();
            var user = await _adminRepository.GetUserAsync(userID);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not Found";
                return response; 
            }

            user.isBlocked = true;
            var result = await _adminRepository.UpdateUserAsync(user); 
            if(result==null)
            {
                response.Success = false;
                response.Message = "User not Found";
                return response;
            }
            response.Success = true;
            response.Data = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber, 
                Role = user.Role, 
                isBlocked = user.isBlocked
            };
            return response; 
        }

        private ItemWithImageResponseDTO BindItemWithReleventImages(Item item, List<ItemImage> itemImages)
        {
            var images = new List<ItemImageResponseDTO>();
            foreach (var image in itemImages)
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


            var result = new ItemWithImageResponseDTO
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
        private IEnumerable<ItemWithImageResponseDTO> ConvertItemAndImagesToResponse(List<Item> items, List<ItemImage> itemImages, List<ItemCategory> itemCategories)
        {
            var itemResponses = new List<ItemWithImageResponseDTO>();

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

                itemResponses.Add(new ItemWithImageResponseDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description,
                    Discount = item.Discount,
                    CreatedAt = item.Created,
                    UpdatedAt = item.Updated,
                    Category = item.ItemCategory.Category, // TODO: fetch from itemCategories
                    Images = imagesForItem
                });


            }

            return itemResponses;
        }
    }
}
