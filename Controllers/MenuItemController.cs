using Microsoft.AspNetCore.Mvc;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService; 
        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var results = await _menuItemService.GetAllMenuItemsAsync(); 
            return Ok(results); 
        }
    }
}