namespace Yummy_Food_API
{
    public class MenuItem
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid ItemCateogryID { get; set;} // Foreign Key 
        public required ItemCategory ItemCategory { get; set; }
    }
}