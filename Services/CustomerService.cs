using Microsoft.Identity.Client;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<string> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail)
        {
            return await _customerRepository.PlaceOrderAsync(orderItemsDTOs, userEmail);
        }
        public async Task<IEnumerable<ItemResponseDTO>> GetAllItemsListAsync()
        {
            var items = await _customerRepository.GetAllItemsAsync();
            var itemCategories = await _customerRepository.GetAllItemCategoriesAsync();
            var itemImages = await _customerRepository.GetAllItemImagesAsync();
            var result = ConvertItemAndImagesToResponse(items, itemImages, itemCategories);
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
                if (item.Discount == 0)
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
