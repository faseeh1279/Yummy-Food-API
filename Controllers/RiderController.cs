using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Yummy_Food_API.Hubs;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Rider")]
    public class RiderController : ControllerBase
    {
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly IRiderService _riderService; 

        public RiderController(IHubContext<OrderHub> hubContext, IRiderService riderService)
        {
            _hubContext = hubContext; 
            _riderService = riderService;
        }

        
        [HttpGet]
        [Route("Get-Data")]
        public async Task<IActionResult> GetData()
        {
            return Ok("Hello World"); 
        }

        [HttpPost]
        [Route("Create-Rider-Profile")]
        public async Task<IActionResult> CreateRiderProfileAsync(RiderProfileDTO riderProfileDTO)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
                var result = await _riderService.AddRiderProfileAsync(riderProfileDTO, userEmail); 
                if(result!= null)
                {
                    return Ok(result); 
                }
                return Ok("Profile Already Exists"); 
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        public async Task<IActionResult> NotifyUser(string email)
        {
            var message = "Order Accepted";

            await _hubContext.Clients.Group(email).SendAsync("Receive Message", "System", message);
            return Ok(new { status = "Message sent to user", email, message });
        }

        [HttpGet]
        [Route("Get-All-Orders")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var result = await _riderService.GetAllOrdersAsync();
            if (result!= null)
            {
                return Ok(result);
            }
            return Ok("No Order Yet"); 
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendData([FromBody] OrderNotificationDTO payload)
        {
            // Broadcast payload to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", payload);
            return Ok(new { Message = "Data sent to SignalR Hub!" });
        }

    }
}
