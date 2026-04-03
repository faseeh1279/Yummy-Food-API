using System.ComponentModel.DataAnnotations;

namespace Yummy_Food_API.Models.Domain
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

    }
}
