using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class ItemListDTO
    {
        public Item Items { get; set; }
        public List<ItemImage> ItemImages { get; set; } 

    }
}
