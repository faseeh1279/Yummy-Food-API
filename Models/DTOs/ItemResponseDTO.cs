namespace Yummy_Food_API.Models.DTOs
{
    public class ItemResponseDTO
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Category { get; set; }
    }
}
