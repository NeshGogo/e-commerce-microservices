using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;

internal interface IBasketStore
{
    Task<CustomerBasket> GetBasketByCustomerIdAsync(string customerId);
    Task CreateCustomerBasketAsync(CustomerBasket customerBasket);
    Task UpdateCustomerBasketAsync(CustomerBasket customerBasket);
    Task DeleteCustomerBasketAsync(string customerId);
}
