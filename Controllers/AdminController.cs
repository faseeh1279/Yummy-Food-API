using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAdminService _adminService;
        public AdminController(IAdminRepository adminRepository,
            IAdminService adminService)
        {
            _adminRepository = adminRepository;
            _adminService = adminService;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] ItemCategoryDTO itemCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminService.AddCategoryAsync(itemCategoryDTO);
            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategoryAsync([FromQuery] Guid CategoryId, [FromBody] ItemCategoryDTO itemCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = await _adminService.UpdateCategoryAsync(CategoryId, itemCategoryDTO);
                return Ok(result);
            }
        }

        [HttpDelete]
        [Route("Delete-Category/{CategoryId}")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid CategoryId)
        {
            if (CategoryId != null)
            {
                var result = await _adminService.DeleteCategoryAsync(CategoryId);
                return Ok(result);
            }
            else
            {
                return BadRequest("Category is Null");
            }
        }

        [HttpGet("Get-All-Categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var result = await _adminService.GetAllCategoriesAsync();
            return Ok(result);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItemAsync([FromBody] ItemDTO itemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminService.AddItemAsync(itemDTO);
            return Ok(result);
        }

        [HttpPut]
        [Route("Update-Item")]
        public async Task<IActionResult> UpdateItemAsync([FromQuery] Guid itemID, [FromBody] ItemDTO itemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = await _adminService.UpdateItemAsync(itemID, itemDTO);
                return Ok(result); 
            }
        }

        [HttpDelete]
        [Route("Delete-Item/{id}")]
        public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid Id)
        {
            var result = await _adminService.DeleteItemAsync(Id);
            return Ok(result);
        }

        [HttpPost]
        [Route("Upload-ItemImage")]
        public async Task<IActionResult> UploadItemImage([FromForm] ItemImageDTO request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                // User Repository to upload Image
                var itemImageModel = new ItemImage
                {
                    ItemId = request.ItemId,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FormFile = request.File   // <-- Important mapping
                };
                var result = await _adminRepository.Upload(itemImageModel);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("Get-Item-Image/{id}")]
        public IActionResult GetImage([FromRoute] Guid id)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Item-Images", id.ToString());

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "image/" + Path.GetExtension(filePath).Trim('.'); // jpg, png, etc.

            return File(imageBytes, contentType);
        }

        [HttpGet]
        [Route("Get-All-Items")]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var result = await _adminService.GetAllItemsAsync();
            return Ok(result);
        }

        [HttpPut]
        [Route("Update-Item-Image")]
        public async Task<IActionResult> UpdateItemImage([FromQuery] Guid ImageId, [FromForm] ItemImageDTO request)
        {
            return Ok("");
        }

        private void ValidateFileUpload(ItemImageDTO request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller file size.");
            }
        }
    }
}