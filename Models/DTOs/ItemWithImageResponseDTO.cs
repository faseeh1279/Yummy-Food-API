namespace Yummy_Food_API.Models.DTOs
{
    public class ItemWithImageResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } 
        public string Description { get; set; } 
        public decimal Discount { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public string Category { get; set; } 
        public List<ItemImageResponseDTO> Images { get; set; }
    }
}
