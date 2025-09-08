using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.DTOs
{
    public class ItemImageDTO
    {
        [Required]
        public Guid ItemId {  get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; } 
        public string? FileDescription { get; set; } 
    }
}   
