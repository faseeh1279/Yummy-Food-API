using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API
{
    public class ItemCategory
    {
        [Key]
        public Guid ID { get; set; }
        public string Category { get; set; } = string.Empty;
        public ICollection<Item> Items { get; set; }

    }
}