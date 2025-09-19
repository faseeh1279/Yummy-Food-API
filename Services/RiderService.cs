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
        public async Task AcceptOrder(Guid orderId, string userEmail)
        {
            var orders = await _riderRepository.GetAllOrdersAsync();
            var order = orders.FirstOrDefault(o => o.Id == orderId); 
            if (order != null)
            {
                var user = await _riderRepository.GetUserAsync(userEmail);
                
                order.OrderStatus = Enums.OrderStatus.Accepted;
                await _riderRepository.AcceptOrderAsync(order); 
                
            }
        }

        public async Task<RiderProfile?> AddRiderProfileAsync(RiderProfileDTO riderProfileDTO, string userEmail) 
        {
            var user = await _riderRepository.GetUserAsync(userEmail); 
            var riderProfile = new RiderProfile
            {
                Id = Guid.NewGuid(), 
                Cnic = riderProfileDTO.Cnic,
                UserId = user.Id
            };
            var result = await _riderRepository.CreateRiderProfileAsync(riderProfile);
            if(result != null)
                return result; 

            return null; 
        }

        public async Task<List<Order>?> GetAllOrdersAsync()
        {
            var orders = await _riderRepository.GetAllOrdersAsync();
            orders.Where(o => o.OrderStatus == Enums.OrderStatus.Pending).ToList();
            if (orders!=null)
                return orders;
            return null; 
        }
    }
}
