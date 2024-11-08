using Basket.Service.Models;

namespace Basket.Test.Domains;

public class CustomerBasketTest
{
    [Fact]
    public void GivenAnEmptyCustomerBasket_WhenCallingAddBasketProduct_ThenProductAddedToBasket()
    {
        var product = new BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new CustomerBasket { CustomerId = "1" };

        customerBasket.AddBasketProduct(product);

        Assert.Contains(product, customerBasket.Products);
    }

    [Fact]
    public void GivenCustomerBasketWithProduct_WhenCallingAddBasketProductWithExistingProduct_ThenBasketUpdated()
    {
        var product = new BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);
        var updatedProduct = product with
        {
            Quantity = 2
        };

        customerBasket.AddBasketProduct(updatedProduct);
        Assert.Contains(updatedProduct, customerBasket.Products);
        Assert.Equal(updatedProduct.Quantity, customerBasket.Products.First().Quantity);
    }
}
