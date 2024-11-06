using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Service.Infrastructure.Redis;

internal class RedisBasketStore : IBasketStore
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _cacheEntryOptions;

    public RedisBasketStore(IDistributedCache cache)
    {
        _cache = cache;
        _cacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(24)
        };
    }

    public async Task CreateCustomerBasketAsync(CustomerBasket customerBasket)
    {
        var serializedBasketProducts = JsonSerializer
            .Serialize(new CustomerBasketCacheModel(customerBasket.Products.ToList()));

        await _cache.SetStringAsync(customerBasket.CustomerId, serializedBasketProducts, _cacheEntryOptions);
    }

    public async Task DeleteCustomerBasketAsync(string customerId) => await _cache.RefreshAsync(customerId);

    public async Task<CustomerBasket> GetBasketByCustomerIdAsync(string customerId)
    {
        var cachedBasketProducts = await _cache.GetStringAsync(customerId);
        
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        if (!string.IsNullOrEmpty(cachedBasketProducts)) 
        {
            var basket = JsonSerializer.Deserialize<CustomerBasketCacheModel>(cachedBasketProducts);

            foreach (var product in basket.Products)
            {
                customerBasket.AddBasketProduct(product);
            }
        }

        return customerBasket;
    }

    public async Task UpdateCustomerBasketAsync(CustomerBasket customerBasket)
    {
        var cachedBasketProducts = await _cache.GetStringAsync(customerBasket.CustomerId);

        if (!string.IsNullOrEmpty(cachedBasketProducts))
        {
            var serializedBasketProducts = JsonSerializer
            .Serialize(new CustomerBasketCacheModel(customerBasket.Products.ToList()));

            await _cache.SetStringAsync(customerBasket.CustomerId, serializedBasketProducts, _cacheEntryOptions);
        }
    }
}
