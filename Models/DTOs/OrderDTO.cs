using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        // Foreign Keys
        public Guid CustomerProfileId { get; set; }
        public CustomerProfile CustomerProfile { get; set; }
        public Guid? RiderProfileId { get; set; }

        public RiderProfile Rider { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
