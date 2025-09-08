using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;
using Yummy_Food_API.Services.Realtime;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin, Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly OrderNotificationService _orderNotificationService;
        //private static HubConnection? _connection;
        //private static List<object> _receivedData = new();
        public CustomerController(
            ICustomerRepository customerRepository,
            ICustomerService customerService,
            OrderNotificationService orderNotificationService)
        {
            _customerRepository = customerRepository;
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
    }
}
