namespace Yummy_Food_API.Models.DTOs
{
    public class ItemCategoryDTO
    {
        public Guid ID { get; set; }
        public string Category { get; set; } = string.Empty;
        public required List<MenuItem> Items { get; set; }
    }
}