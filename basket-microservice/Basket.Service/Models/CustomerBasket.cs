namespace Basket.Service.Models;

internal class CustomerBasket
{
    private readonly HashSet<BasketProduct> _products = [];
    public IEnumerable<BasketProduct> Products => _products;
    public required string CustomerId { get; init; }

    public decimal BasketTotal
    {
        get
        {
            return _products.Select(p => p.Quantity * p.ProductPrice).Sum();
        }
    }


    public void AddBasketProduct(BasketProduct product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.ProductId.Equals(product.ProductId));

        if (existingProduct is null)
        {
            _products.Add(product);
        }
        else
        {
            _products.Remove(existingProduct);
            _products.Add(product);
        }
    }

    public void RemoveBasketProduct(string productId) => _products.RemoveWhere(p => p.ProductId == productId);
}
