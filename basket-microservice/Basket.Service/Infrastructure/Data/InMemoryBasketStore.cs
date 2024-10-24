using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;

internal class InMemoryBasketStore : IBasketStore
{
    private static readonly Dictionary<string, CustomerBasket> Basket = [];

    public void CreateCustomerBasket(CustomerBasket customerBasket) =>
        Basket[customerBasket.CustomerId] = customerBasket;

    public CustomerBasket GetBasketByCustomerId(string customerId) => 
        Basket.TryGetValue(customerId, out var customer) ? customer 
        : new CustomerBasket { CustomerId = customerId };

}
