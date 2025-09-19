using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;

namespace Yummy_Food_API.Models.Domain
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string Address { get; set; } 
        // Foreign Keys
        public Guid CustomerProfileId { get; set; }
        public CustomerProfile CustomerProfile { get; set; }
        public Guid? RiderProfileId { get; set; }
        public RiderProfile Rider { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}
