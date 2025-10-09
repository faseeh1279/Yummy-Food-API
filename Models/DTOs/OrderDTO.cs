using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class OrderDTO 
    {
        public Guid OrderID { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string Address { get; set; }
        // Foreign Keys
        public Guid CustomerProfileId { get; set; }
        public Guid? RiderProfileId { get; set; }
    }
}
