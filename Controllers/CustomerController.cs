using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
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
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly OrderNotificationService _orderNotificationService;
        //private static HubConnection? _connection;
        //private static List<object> _receivedData = new();
        public CustomerController(
            ICustomerService customerService,
            OrderNotificationService orderNotificationService)
        {
            _customerService = customerService;
            _orderNotificationService = orderNotificationService;
        }


        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            return Ok(_orderNotificationService.ReceivedData);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] object result)
        {
            //var result = new
            //{
            //    userId: UserId,
            //    Message: message
            //};
            await _orderNotificationService.BroadCastMessageAsync(result);
                return Ok("Message sent!");
        }

        [HttpGet]
        [Route("Get-All-Items")]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var result = await _customerService.GetAllItemsListAsync();
            if (result!=null)
            {
                return Ok(result); 
            }
            else
            {
                return BadRequest("No Result"); 
            }
        }

        [HttpPost]
        [Route("Place-Order")]
        public async Task<IActionResult> PlaceOrderAsync(List<OrderItemsDTO> orderItemsDTO)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
                //var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var result = await _customerService.PlaceOrderAsync(orderItemsDTO, userEmail);

                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }
    }
}
