using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;
using Yummy_Food_API.Services.Realtime;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer, Admin")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<AppHub> _hubContext;
        public CustomerController(
            ICustomerService customerService,
            IHubContext<AppHub> hubContext)
        {
            _customerService = customerService;
            _hubContext = hubContext;
        }

        //[HttpPost]
        //[Route("send-to-user")]
        //public async Task<IActionResult> SendToUser(string userId, string message)
        //{
        //    await _hubContext.Clients.User(userId).SendAsync("OrderAccepted", message);
        //    return Ok(new { Status = "Sent to User", User = userId, Data = message });
        //}

        [HttpGet]
        [Route("Get-All-Items")]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var result = await _customerService.GetAllItemsListAsync();
            if (result == null)
                return BadRequest(result!.Message); 
            return Ok(result.Data);
        }

        [HttpGet]
        [Route("Get-Order-Status")]
        public async Task<IActionResult> GetOrderStatus()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            var result = await _customerService.GetCurrentPendingOrderAsync(userEmail);
            if (result.Success)
                return Ok(result.Data); 
            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("Place-Order")]
        public async Task<IActionResult> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTO, string Address)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
                var result = await _customerService.PlaceOrderAsync(orderItemsDTO, userEmail, Address);
                if (result.Success)
                    return Ok(result.Data); 
                return BadRequest(result.Message);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        [Route("Get-Past-Order-History")]
        public async Task<IActionResult> GetOrderHistoryAsync()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            var result = await _customerService.GetDeliveredOrderHistory(userEmail);
            if (result.Success)
                return Ok(result.Data); 
            
            return BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("Cancel-Order")]
        public async Task<IActionResult> DeleteOrderAsync([FromQuery] Guid OrderId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            var result = await _customerService.DeleteOrderAsync(OrderId, userEmail);
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result.Message); 
        }

        [HttpGet]
        [Route("Get-Current-Pending-Order")]
        public async Task<IActionResult> GetCurrentPendingOrderAsync()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            var result = await _customerService.GetCurrentPendingOrderAsync(userEmail);
            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result.Message); 
        }
    }
}
