using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.Domain
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public Guid ItemCategoryId { get; set; } // Foreign Key to ItemCategory
        public ItemCategory ItemCategory { get; set; }
        public ICollection<ItemImage> Images { get; set; } 
    }
}
