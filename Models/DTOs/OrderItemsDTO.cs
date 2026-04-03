using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.DTOs
{
    public class OrderItemsDTO
    {
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
