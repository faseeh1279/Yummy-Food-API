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


        //[HttpGet("messages")]
        //public IActionResult GetMessages()
        //{
        //    return Ok(_orderNotificationService.ReceivedData);
        //}

        //[HttpPost("send")]
        //public async Task<IActionResult> SendMessage([FromBody] object result)
        //{
        //    //var result = new
        //    //{
        //    //    userId: UserId,
        //    //    Message: message
        //    //};
        //    await _orderNotificationService.BroadCastMessageAsync(result);
        //        return Ok("Message sent!");
        //}


        [HttpPost]
        [Route("Place-Order")]
        public async Task<IActionResult> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTO, string Address)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
                var result = await _customerService.PlaceOrderAsync(orderItemsDTO, userEmail, Address);

                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("Delete-Order")]
        public async Task<IActionResult> DeleteOrderAsync([FromQuery] Guid OrderId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            var result = await _customerService.DeleteOrderAsync(OrderId, userEmail);
            return Ok(result); 
        }

        
        //[HttpPost("notify-riders")]
        //public async Task<IActionResult> NotifyRiders(string message)
        //{
        //    await _hubContext.Clients.Group("Rider").SendAsync("ReceiveMessage", "System", message);
        //    return Ok(new { status = "Message sent to all riders", message });
        //}
    }
}
