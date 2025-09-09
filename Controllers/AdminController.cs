using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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
        private readonly ApplicationDBContext _dbContext;
        public AdminController(
            IAdminRepository adminRepository,
            IAdminService adminService,
            ApplicationDBContext dbContext 
            )
        {
            _adminRepository = adminRepository;
            _adminService = adminService;
            _dbContext = dbContext;
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
                var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == request.ItemId); 
                if(item!= null)
                {
                    // User Repository to upload Image
                    var itemImageModel = new ItemImage
                    {
                        Id = Guid.NewGuid(),
                        ItemId = request.ItemId,
                        FileName = item.Name,
                        FileDescription = item.Description,
                        FileExtension = Path.GetExtension(request.File.FileName),
                        FileSizeInBytes = request.File.Length,
                        FormFile = request.File   // <-- Important mapping
                    };
                    var result = await _adminRepository.Upload(itemImageModel);
                    return Ok(result);
                }
                else
                {
                    return BadRequest($"Item with {request.ItemId} does not exists!"); 
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("Get-Item-Image/{ImageId}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid ImageId)
        {
            var image = await _dbContext.ItemImages.FirstOrDefaultAsync(i => i.Id == ImageId);
            if(image != null)
            {
                var fileNameWithExt = image.Id + image.FileExtension; 
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Item-Images", fileNameWithExt);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();
                var imageBytes = System.IO.File.ReadAllBytes(filePath);
                var contentType = "image/" + Path.GetExtension(filePath).Trim('.'); // jpg, png, etc.
                return File(imageBytes, contentType);
            }
            else
            {
                return BadRequest("Item Not Found"); 
            }
                        

        }

        [HttpPut]
        [Route("Update-Item-Image/{ImageId}")]
        public async Task<IActionResult> UpdateItemImage([FromRoute] Guid ImageId, IFormFile itemImage)
        {
            var image = await _dbContext.ItemImages.FirstOrDefaultAsync(i => i.Id == ImageId); 
            if(image != null)
            {
                var fileNameWithExt = image.Id + image.FileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Item-Images", fileNameWithExt);
                var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Item-Images", fileNameWithExt); 
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    image.FileSizeInBytes = itemImage.Length;
                    image.FileExtension = Path.GetExtension(itemImage.FileName);
                    image.FormFile = itemImage;

                    // Save the file
                    using var stream = new FileStream(localFilePath, FileMode.Create);
                    await image.FormFile.CopyToAsync(stream);

                    await _dbContext.SaveChangesAsync();
                    return Ok("Image Updated Successfully!"); 
                }
                return BadRequest("Image Not Found"); 
            }
            else
            {
                return BadRequest("Image Not Found"); 
            }
        }

        [HttpDelete]
        [Route("Delete-Item-Image/{ImageId}")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute] Guid ImageId)
        {
            var image = await _dbContext.ItemImages.FindAsync(ImageId); 
            if (image != null)
            {
                var fileNameWithExt = image.Id + image.FileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Item-Images", fileNameWithExt);
                var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Item-Images", fileNameWithExt);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);

                    _dbContext.ItemImages.Remove(image); 
                    await _dbContext.SaveChangesAsync();
                    return Ok("Image Removed Successfully!");
                }
                return BadRequest("Image Not Found");
            }
            return BadRequest("Image Not Found"); 
        }

        [HttpGet]
        [Route("Get-All-Items")]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var result = await _adminService.GetAllItemsAsync();
            return Ok(result);
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