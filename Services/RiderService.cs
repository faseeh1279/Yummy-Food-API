using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Services
{
    public class RiderService : IRiderService
    {
        private readonly IRiderRepository _riderRepository;
        public RiderService(IRiderRepository riderRepository)
        {
            _riderRepository = riderRepository;
        }
        public async Task<ServiceResponse<OrderDTO>> AcceptOrder(Guid orderId, string userEmail)
        {
            var response = new ServiceResponse<OrderDTO>();

            var user = await _riderRepository.GetUserAsync(userEmail);

            var riderProfile = await _riderRepository.GetRiderProfileAsync(user.Id);

            if (riderProfile == null)
            {
                response.Success = false;
                response.Message = "Rider Profile Not Found. Please Create Rider Profile First.";
                return response;
            }
            var orders = await _riderRepository.GetAllOrdersAsync();
            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                response.Success = false;
                response.Message = "No Pending Order Found with this ID";
                return response;
            }

            order.OrderStatus = Enums.OrderStatus.Accepted;
            order.RiderProfileId = riderProfile.Id;
            await _riderRepository.UpdateOrderAsync(order);
            response.Data = new OrderDTO
            {
                OrderID = order.Id,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                Address = order.Address,
                CustomerProfileId = order.CustomerProfileId,
                RiderProfileId = order.RiderProfileId
            };
            response.Success = true;
            response.Message = "Order Accepted Successfully";
            return response;
        }
        public async Task<ServiceResponse<OrderDTO>> CompleteOrderAsync(Guid orderId, string userEmail)
        {
            var response = new ServiceResponse<OrderDTO>();
            //var user = await _riderRepository.GetUserAsync(userEmail);
            var user = await _riderRepository.GetUserAsync(userEmail); 
            var riderProfile = await _riderRepository.GetRiderProfileAsync(user.Id); 
            if (riderProfile == null)
            {
                response.Success = false;
                response.Message = "Rider Profile Not Found. Please Create Rider Profile First.";
                return response;
            }
            var order = await _riderRepository.FetchAcceptedOrderAsync(riderProfile.Id);
            if (order == null)
            {
                response.Success = false;
                response.Message = "No Accepted Order Found";
                return response;
            }
            order.OrderStatus = Enums.OrderStatus.Delivered;
            order.CompletedAt = DateTime.UtcNow;
            var result = await _riderRepository.UpdateOrderAsync(order);
            if (result == null)
            {
                response.Success = false;
                response.Message = "Error While Completing Order";
                return response;
            }
            response.Data = new OrderDTO
            {
                OrderID = order.Id,
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                Address = order.Address,
                CustomerProfileId = order.CustomerProfileId,
                RiderProfileId = order.RiderProfileId
            };
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponse<RiderProfile>> AddRiderProfileAsync(RiderProfileDTO riderProfileDTO, string userEmail)
        {
            var response = new ServiceResponse<RiderProfile>();
            var user = await _riderRepository.GetUserAsync(userEmail);
            var riderProfile = new RiderProfile
            {
                Id = Guid.NewGuid(),
                Cnic = riderProfileDTO.Cnic,
                UserId = user.Id
            };
            var result = await _riderRepository.CreateRiderProfileAsync(riderProfile);
            if (result != null)
            {
                response.Data = result;
                response.Success = true;
                response.Message = "Rider Profile Created Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "Rider Profile Already Exists";
            return response;
        }
        public async Task<ServiceResponse<List<OrderDTO>>> GetAllOrdersAsync(string userEmail)
        {
            var response = new ServiceResponse<List<OrderDTO>>();
            var user = await _riderRepository.GetUserAsync(userEmail);
            var riderProfile = await _riderRepository.GetRiderProfileAsync(user.Id);
            if (riderProfile == null)
            {
                response.Success = false;
                response.Message = "Rider Profile not found";
                return response;
            }
            var orders = await _riderRepository.GetAllOrdersAsync();
            orders = orders.Where(o => o.OrderStatus == Enums.OrderStatus.Pending && o.RiderProfileId == null).ToList();
            if (orders != null)
            {
                response.Data = orders
                    .Select(o => new OrderDTO
                    {
                        OrderID = o.Id,
                        CreatedAt = o.CreatedAt,
                        TotalPrice = o.TotalPrice,
                        OrderStatus = o.OrderStatus,
                        Address = o.Address,
                        CustomerProfileId = o.CustomerProfileId,
                        RiderProfileId = o.RiderProfileId
                    }).ToList();
                response.Success = true;
                response.Message = "Orders Retrieved Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "No Orders Found";
            return response;
        }
        public async Task<ServiceResponse<OrderDTO>> DeleteOrderAsync(string userEmail)
        {
            var response = new ServiceResponse<OrderDTO>();
            var user = await _riderRepository.GetUserAsync(userEmail);
            var riderProfile = await _riderRepository.GetRiderProfileAsync(user.Id);
            if (riderProfile == null)
            {
                response.Success = false; 
                response.Message = "Rider Profile Not Found. Please Create Rider Profile First.";
                return response; 
            }
            var order = await _riderRepository.FetchPendingOrdersAsync(riderProfile.Id);
            if (order == null)
            {
                response.Success = false;
                response.Message = "No Pending Order Found";
                return response;
            }
            if(DateTime.UtcNow - order.CreatedAt > TimeSpan.FromMinutes(2))
            {
                response.Success = false;
                response.Message = "Cannot Cancel Order after 2 minutes of placement";
                return response;
            }
            order.OrderStatus = Enums.OrderStatus.Cancelled;
            var result = await _riderRepository.UpdateOrderAsync(order); 
            if(result  == null)
            {
                response.Success = false;
                response.Message = "Error While Cancelling Order";
                return response;
            }
            response.Data = new OrderDTO
            {
                OrderID = order.Id,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                Address = order.Address,
                CustomerProfileId = order.CustomerProfileId,
                RiderProfileId = order.RiderProfileId
            };
            response.Success = true;
            response.Message = "Order Cancelled Successfully";
            return response; 
        }
        public async Task<ServiceResponse<OrderDTO>> GetAcceptedOrderDetail(string userEmail)
        {
            var response = new ServiceResponse<OrderDTO>();
            var user = await _riderRepository.GetUserAsync(userEmail);
            var riderProfile = await _riderRepository.GetRiderProfileAsync(user.Id);
            if (riderProfile == null)
            {
                response.Success = false;
                response.Message = "Rider Profile Not Found. Please Create Rider Profile First.";
                return response;
            }
            var order = await _riderRepository.FetchAcceptedOrderAsync(riderProfile.Id);
            if (order == null)
            {
                response.Success = false;
                response.Message = "No Accepted Order Found";
                return response;
            }
            response.Data = new OrderDTO
            {
                OrderID = order.Id,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                Address = order.Address,
                CustomerProfileId = order.CustomerProfileId,
                RiderProfileId = order.RiderProfileId
            };
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponse<List<OrderDTO>>> GetCompletedOrders(string userEmail)
        {
            var response = new ServiceResponse<List<OrderDTO>>();
            var user = await _riderRepository.GetUserAsync(userEmail);
            var riderProfile = await _riderRepository.GetRiderProfileAsync(user.Id);
            if (riderProfile == null)
            {
                response.Success = false;
                response.Message = "Rider Profile not found";
                return response;
            }
            var orders = await _riderRepository.GetAllOrdersAsync();
            orders = orders.Where(o => o.OrderStatus == Enums.OrderStatus.Delivered && o.RiderProfileId == riderProfile.Id).ToList();
            if (orders != null)
            {
                response.Data = orders
                    .Select(o => new OrderDTO
                    {
                        OrderID = o.Id,
                        CreatedAt = o.CreatedAt,
                        CompletedAt = o.CompletedAt,
                        TotalPrice = o.TotalPrice,
                        OrderStatus = o.OrderStatus,
                        Address = o.Address,
                        CustomerProfileId = o.CustomerProfileId,
                        RiderProfileId = o.RiderProfileId
                    }).ToList();
                response.Success = true;
                response.Message = "Orders Retrieved Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "No Orders Found";
            return response;
        }

        //public async Task<bool> VerifyRiderProfile(string userEmail)
        //{
        //    var result = await GetRiderProfileAsync(userEmail);
        //    if (result != null)
        //        return true;
        //    return false;
        //}
    }
}
