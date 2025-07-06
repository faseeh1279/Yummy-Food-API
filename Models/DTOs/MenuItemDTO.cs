namespace Yummy_Food_API.Models.DTOs
{
    public class MenuItemDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Instead of full object, send just needed category info
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}