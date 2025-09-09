using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Yummy_Food_API.Models.Domain
{
    public class ItemImage
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; } 
        [NotMapped]
        public IFormFile FormFile { get; set; } 
        public string FileName { get; set; } 
        public string FileDescription { get; set; } 
        public string FileExtension { get; set; } 
        public long FileSizeInBytes { get; set; } 
        public string FilePath { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
    }
}
