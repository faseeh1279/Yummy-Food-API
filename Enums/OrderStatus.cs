namespace Yummy_Food_API.Enums
{
    public enum OrderStatus
    {
        Pending,        // Order placed but not yet processed
        Accepted,       // Order accepted by system/admin/rider
        Rejected,       // Order rejected due to invalidity, stock, etc.
        InTransit,      // (Optional) Order is being delivered
        Delivered,      // Order successfully delivered
        Cancelled       // Order cancelled by user or system
    }
}
