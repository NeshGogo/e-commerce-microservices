using Order.Service.Models;

namespace Order.Service.Infrastructure.Data;

internal interface IOrderStore
{
    Task CreateOrderAsync(Models.Order order);
    Task<Models.Order?> GetCustomerOrderByIdAsync(string customerId, string orderId);
}
