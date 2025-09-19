using Microsoft.EntityFrameworkCore;
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
        public async Task<string> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail, string Address)
        {
            var user = await _customerRepository.GetUserAsync(userEmail);
            if (user == null) return "User not found";

            var customerProfile = await _customerRepository.GetCustomerProfileAsync(user.Id);
            if (customerProfile == null) return "Customer profile not found";

            var existingOrder = await _customerRepository.GetPendingOrderByCustomerAsync(customerProfile.Id);
            if (existingOrder != null)
            {
                return "You already have a pending order.";
            }

            // Create new order
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerProfileId = customerProfile.Id,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = 0, // will update later
                Address = Address,
                OrderStatus = Enums.OrderStatus.Pending
            };

            await _customerRepository.PlaceOrderAsync(newOrder);

            // Fetch only relevant items
            var itemIds = orderItemsDTOs.Select(d => d.ItemId).ToList();
            var items = (await _customerRepository.GetAllItemsAsync())
                            .Where(i => itemIds.Contains(i.Id))
                            .ToList();

            var orderItems = new List<OrderItem>();
            decimal totalPrice = 0;

            foreach (var dto in orderItemsDTOs)
            {
                var item = items.FirstOrDefault(i => i.Id == dto.ItemId);
                if (item == null) continue;

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    ItemId = item.Id,
                    Quantity = dto.Quantity,
                    UnitPrice = item.Price
                };

                orderItems.Add(orderItem);
                totalPrice += item.Price * dto.Quantity;
            }

            // Save order items
            await _customerRepository.AddOrderItemsRangeAsync(orderItems);

            // Update total price
            newOrder.TotalPrice = totalPrice;
            await _customerRepository.UpdateOrderAsync(newOrder);

            return "Order placed successfully.";
        }
        public async Task<IEnumerable<ItemResponseDTO>> GetAllItemsListAsync()
        {
            var items = await _customerRepository.GetAllItemsAsync();
            var itemCategories = await _customerRepository.GetAllCategoriesAsync();
            var itemImages = await _customerRepository.GetAllItemImagesAsync();
            var result = ConvertItemAndImagesToResponse(items, itemImages, itemCategories);
            return result;
        }
        public async Task<Order?> DeleteOrderAsync(Guid orderId, string userEmail)
        {
            var result = await _customerRepository.DeleteOrderAsync(orderId);
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
