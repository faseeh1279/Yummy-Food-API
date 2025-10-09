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
        public async Task<ServiceResponse<OrderDTO>> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTOs, string userEmail, string Address)
        {
            var response = new ServiceResponse<OrderDTO>(); 
            var user = await _customerRepository.GetUserAsync(userEmail);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response; 
            }

            var customerProfile = await _customerRepository.GetCustomerProfileAsync(user.Id);
            if (customerProfile == null)
            {
                response.Success = false;
                response.Message = "Customer profile not found";
                return response; 
            }

            var existingOrder = await _customerRepository.GetPendingOrderByCustomerProfileAsync(customerProfile.Id);
            if (existingOrder != null)
            {
                response.Success = false;
                response.Message = "You already have a pending order.";
                return response;
            }

            // Create new order
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerProfileId = customerProfile.Id,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = 0,
                Address = Address,
                OrderStatus = Enums.OrderStatus.Pending
            };

            await _customerRepository.PlaceOrderAsync(newOrder);

            // Fetch only relevant items
            var itemIds = orderItemsDTOs.Select(d => d.ItemId).ToList();
            var items = await _customerRepository.GetAllItemsAsync();
            if (items == null)
            {
                response.Success = false;
                response.Message = "No items available";
                return response;
            }

            items = items.Where(i => itemIds.Contains(i.Id))
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
            response.Data = new OrderDTO
            {
                OrderID = newOrder.Id,
                CreatedAt = newOrder.CreatedAt,
                TotalPrice = newOrder.TotalPrice,
                OrderStatus = newOrder.OrderStatus,
                Address = newOrder.Address,
                CustomerProfileId = newOrder.CustomerProfileId,
                RiderProfileId = newOrder.RiderProfileId
            };
            response.Success = true;
            return response; 
        }
        public async Task<ServiceResponse<IEnumerable<ItemWithImageResponseDTO>>> GetAllItemsListAsync()
        {
            var response = new ServiceResponse<IEnumerable<ItemWithImageResponseDTO>>();
            var items = await _customerRepository.GetAllItemsAsync();
            var itemCategories = await _customerRepository.GetAllCategoriesAsync();
            var itemImages = await _customerRepository.GetAllItemImagesAsync();
            if (items == null || itemCategories == null)
            {
                response.Success = false;
                response.Message = "No Items Found";
                return response;
            }
            var result = ConvertItemAndImagesToResponse(items, itemImages, itemCategories);
            response.Success = true;
            response.Data = result;
            return response;
        }
        public async Task<ServiceResponse<OrderDTO>> DeleteOrderAsync(Guid orderId, string userEmail)
        {
            var response = new ServiceResponse<OrderDTO>();
            var orders = await _customerRepository.GetAllOrdersAsync();
            if (orders == null)
            {
                response.Success = false;
                response.Message = "No Orders Found";
                return response;
            }
            var order = orders.FirstOrDefault(o => o.Id == orderId); 
            if(order == null)
            {
                response.Success = false;
                response.Message = "Order Not Found";
                return response;
            }
            if (DateTime.UtcNow - order.CreatedAt > TimeSpan.FromMinutes(5))
            {
                response.Success = false;
                response.Message = "You can only cancel an order within 5 minutes of placing it.";
                return response;
            }

            var result = await _customerRepository.DeleteOrderAsync(orderId);
            if (result != null)
            {
                response.Success = true;
                response.Data = new OrderDTO
                {
                    OrderID = result.Id,
                    CreatedAt = result.CreatedAt,
                    TotalPrice = result.TotalPrice,
                    OrderStatus = result.OrderStatus,
                    Address = result.Address,
                    CustomerProfileId = result.CustomerProfileId,
                    RiderProfileId = result.RiderProfileId
                };
                return response;
            }
            response.Success = false; 
            response.Message = "Order Not Found";
            return response;
        }
        public async Task<ServiceResponse<List<OrderDTO>>> GetDeliveredOrderHistory(string userEmail)
        {
            var response = new ServiceResponse<List<OrderDTO>>(); 
            var user = await _customerRepository.GetUserAsync(userEmail);
            var customerProfile = await _customerRepository.GetCustomerProfileAsync(user!.Id); 
            if(customerProfile == null)
            {
                response.Success = false;
                response.Message = "Customer Profile Does not Exist";
                return response; 
            }
            else
            {
                var result = await _customerRepository.GetDeliveredOrderHistoryAsync(customerProfile.Id);
                var finalResult = new List<OrderDTO>();
                response.Success = false;
                response.Message = "No Orders Found"; 
                if (result == null)
                    return response; 

                foreach(var order in result)
                {
                    finalResult.Add(new OrderDTO
                    {
                        OrderID = order.Id, 
                        CompletedAt = order.CompletedAt, 
                        CreatedAt = order.CreatedAt, 
                        TotalPrice = order.TotalPrice,
                        OrderStatus = order.OrderStatus, 
                        Address = order.Address, 
                        CustomerProfileId = order.CustomerProfileId, 
                        RiderProfileId = order.RiderProfileId
                    });  
                }
                response.Success = true;
                response.Data = finalResult;
                return response; 
                
            }
        }
        public async Task<ServiceResponse<OrderDTO>> GetCurrentPendingOrderAsync(string userEmail)
        {
            var response = new ServiceResponse<OrderDTO>();
            var user = await _customerRepository.GetUserAsync(userEmail);
            var customerProfile = await _customerRepository.GetCustomerProfileAsync(user!.Id);
            if (customerProfile == null)
            {
                response.Success = false;
                response.Message = "Custoemr Profile Does not Exist";
                return response;
            }

            var result = await _customerRepository.GetPendingOrderByCustomerProfileAsync(customerProfile.Id);
            if (result == null)
            {
                response.Success = false;
                response.Message = "No Pending Order Found";
                return response;
            }
            response.Success = true;
            response.Data = new OrderDTO
            {
                OrderID = result.Id,
                CreatedAt = result.CreatedAt,
                TotalPrice = result.TotalPrice,
                OrderStatus = result.OrderStatus,
                Address = result.Address,
                CustomerProfileId = result.CustomerProfileId,
                RiderProfileId = result.RiderProfileId
            };
            return response;
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
                    Category = item.ItemCategory.Category, // TODO: fetch from itemCategories
                    Images = imagesForItem
                });

            }

            var result = itemResponses.OrderByDescending(i => i.CreatedAt); 

            return result;
        }
    }

}
