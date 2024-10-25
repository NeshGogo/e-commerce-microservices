namespace Order.Service.Models;

internal class Order
{
    private readonly List<OrderProduct> _orderProducts = [];
    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts.AsReadOnly();
    public required string CustomerId { get; init; }

    public Guid OrderId { get; private set; }
    public DateTime OrderDate { get; private set; }

    public Order(Guid orderId, DateTime orderDate)
    {
        OrderDate = orderDate;
        OrderId = orderId;
    }

    public void AddOrderProduct(string productId, int quantity)
    {
        var existingOrderProduct = _orderProducts.SingleOrDefault(p => p.ProductId == productId);

        if (existingOrderProduct != null)
        {
            existingOrderProduct.AddQuantity(quantity);
        }
        else
        {
            var orderProduct = new OrderProduct { ProductId = productId };
            orderProduct.AddQuantity(quantity);
            _orderProducts.Add(orderProduct);
        }
    }
}
