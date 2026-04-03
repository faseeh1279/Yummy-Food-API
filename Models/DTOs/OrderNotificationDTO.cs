using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class OrderNotificationDTO
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        // Foreign Keys
        public string RiderName { get; set; } 
        public string RiderPhone { get; set; } 
        public ICollection<Item> Items { get; set; }
    }
}
