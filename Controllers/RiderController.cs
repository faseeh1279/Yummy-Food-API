using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiderController : ControllerBase
    {
        [HttpGet]
        [Route("Get-All-Orders")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            return Ok(""); 
        }
    }
}
