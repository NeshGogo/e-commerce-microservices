namespace Order.Service.Models;

internal class Order
{
    public required string CustomerId { get; init; }
    public Guid OrderId { get; private set; }
    public DateTime OrderDate { get; private set; }

    public ICollection<OrderProduct> OrderProducts { get; set; }

    public Order()
    {
        OrderDate = DateTime.UtcNow;
        OrderId = Guid.NewGuid();
    }

    public void AddOrderProduct(string productId, int quantity)
    {
        var existingOrderProduct = OrderProducts.SingleOrDefault(p => p.ProductId == productId);

        if (existingOrderProduct != null)
        {
            existingOrderProduct.AddQuantity(quantity);
        }
        else
        {
            var orderProduct = new OrderProduct { ProductId = productId };
            orderProduct.AddQuantity(quantity);
            OrderProducts.Add(orderProduct);
        }
    }
}
