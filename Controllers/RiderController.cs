using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Yummy_Food_API.Hubs;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin, Rider")]
    public class RiderController : ControllerBase
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public RiderController(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext; 
        }

        //[HttpPost("Apply-For-Rider")]
        //public async Task<IActionResult> ApplyForRider()
        //{
        //    //await _hubContext.Clients.User().SendAsync("ReceiveMessage", payload); 
        //    return Ok(""); 
        //}

        [HttpPost("send")]
        public async Task<IActionResult> SendData([FromBody] OrderNotificationDTO payload)
        {
            // Broadcast payload to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", payload);
            return Ok(new { Message = "Data sent to SignalR Hub!" });
        }
    }
}
