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
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAdminService _adminService;
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        public AdminController(IAdminRepository adminRepository, 
            IAdminService adminService, 
            ApplicationDBContext dbContext, 
            IWebHostEnvironment webHostEnvironment)
        {
            _adminRepository = adminRepository;
            _adminService = adminService;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItemAsync([FromBody] ItemDTO itemDTO)
        {
            if (itemDTO == null)
            {
                return BadRequest("Item data is null");
            }
            var result = await _adminService.AddItemAsync(itemDTO);
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
        [Route("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _adminService.GetAllOrdersAsync();
            return Ok(); 
        }

        [HttpGet]
        [Route("Complaints")]
        public async Task<IActionResult> GetComplaints()
        {
            var complaints = await _adminService.GetAllComplaintsAsync();
            return Ok(complaints);
        }

        [HttpGet]
        [Route("Riders")]
        public async Task<IActionResult> GetRidersAsycn()
        {
            return Ok(""); 
        }

        [HttpPost]
        [Route("ViewRiderProfile")]
        public async Task<IActionResult> ViewRiderProfileAsync([FromBody] Guid riderId)
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
