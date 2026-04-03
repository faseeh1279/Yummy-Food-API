using Microsoft.EntityFrameworkCore;
using Yummy_Food_API;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories;
public class OrdersRepository
{
    private readonly ApplicationDBContext _dbContext;
    public OrdersRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext; 
    }
    public async Task<List<Order>?> FetchCompletedOrdersAsync()
    {
        var result = await _dbContext.Orders.Where(o => o.OrderStatus == Enums.OrderStatus.Delivered).ToListAsync();
        if (result != null)
            return result;
        return null;
    }
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        var result = await _dbContext.Orders.ToListAsync();
        return result;
    }
    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        var result = await _dbContext.SaveChangesAsync();
        if (result == 0)
            return null;
        return order;
    }
    public async Task<Order?> FetchPendingOrdersAsync(Guid riderProfileID)
    {
        var result = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderStatus == Enums.OrderStatus.Pending && o.RiderProfileId == riderProfileID);
        if (result != null)
            return result;
        return null;
    }
    public async Task<Order?> FetchAcceptedOrderAsync(Guid riderProfileID)
    {
        var result = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderStatus == Enums.OrderStatus.Accepted && o.RiderProfileId == riderProfileID);
        if (result != null)
            return result;
        return null;
    }
    public async Task DeclineOrderAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<Order>?> GetCompletedOrdersAsync(Guid riderProfileId)
    {
        var result = await _dbContext.Orders.Where(o => o.RiderProfileId == riderProfileId &&
        o.OrderStatus == Enums.OrderStatus.Delivered).ToListAsync();
        if (result.Count > 0)
        {
            return result;
        }
        return null;
    }
    public async Task<Order?> DeleteOrderAsync(Guid orderId)
    {
        var result = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (result != null)
        {
            _dbContext.Orders.Remove(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        return null;
    }
    public async Task<Order> PlaceOrderAsync(Order order)
    {
        var result = await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }
}
