using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API
{
    public class ItemCategory
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;
        // Navigation property
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}