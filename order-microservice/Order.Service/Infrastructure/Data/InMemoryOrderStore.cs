
namespace Order.Service.Infrastructure.Data
{
    internal class InmemoryOrderStore : IOrderStore
    {
        private static readonly Dictionary<string, Models.Order> Orders = [];
        public void CreateOrder(Models.Order order) =>
            Orders[$"{order.CustomerId}-{order.OrderId}"] = order;
    }
}
