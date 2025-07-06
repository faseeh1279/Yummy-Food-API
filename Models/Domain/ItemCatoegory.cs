namespace Yummy_Food_API
{
    public class ItemCategory
    {
        public Guid ID { get; set; }
        public string Category { get; set; } = string.Empty; 
        public required List<MenuItem> Items { get; set; } 
    }
}