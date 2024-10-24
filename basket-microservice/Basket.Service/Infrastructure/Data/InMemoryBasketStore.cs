using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;

internal class InMemoryBasketStore : IBasketStore
{
    private static readonly Dictionary<string, CustomerBasket> Basket = [];

    public void CreateCustomerBasket(CustomerBasket customerBasket) =>
        Basket[customerBasket.CustomerId] = customerBasket;

    public void DeleteCustomerBasket(string customerId) => Basket.Remove(customerId);

    public CustomerBasket GetBasketByCustomerId(string customerId) =>
        Basket.TryGetValue(customerId, out var customer) ? customer
        : new CustomerBasket { CustomerId = customerId };

    public void UpdateCustomerBasket(CustomerBasket customerBasket)
    {
        if (Basket.TryGetValue(customerBasket.CustomerId, out _))
        {
            Basket[customerBasket.CustomerId] = customerBasket;
        }
        else
        {
            CreateCustomerBasket(customerBasket);
        }
    }
}
