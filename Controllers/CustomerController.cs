using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService; 
        public CustomerController(ICustomerRepository customerRepository, ICustomerService customerService )
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
        }
        [HttpGet]
        [Route("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _customerService.GetAllItemAsync();
            if (result != null)
            {
                return Ok(result); 
            }
            else
            {
                return NotFound("No items found"); 
            }
        }
    }
}
