using Basket.Service.ApiModels;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("{customerId}", GetBasket);

        app.MapPost("{customerId}", CreateBasket);

        app.MapPut("{customerId}", AddBasketProduct);

        app.MapDelete("{customerId}/{productId}", DeleteBasketProduct);

        app.MapDelete("{customerId}", DeleteBasket);
    }

    internal static  async Task<CustomerBasket> GetBasket(string customerId, [FromServices] IBasketStore basketStore) => 
        await basketStore.GetBasketByCustomerIdAsync(customerId);

    internal static async Task<IResult> CreateBasket(IBasketStore basketStore, IDistributedCache cache, string customerId,
            CreateBasketRequest createBasketRequest)
    {
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        var cachedProductPrice = decimal.Parse(await cache.GetStringAsync(createBasketRequest.ProductId));

        customerBasket.AddBasketProduct(new BasketProduct(
             createBasketRequest.ProductId,
             createBasketRequest.ProductName,
             cachedProductPrice
        ));

        await basketStore.CreateCustomerBasketAsync(customerBasket);

        return TypedResults.Created();
    }

    internal static async Task<IResult> AddBasketProduct(IBasketStore basketStore, IDistributedCache cache, string customerId,
            AddBasketProductRequest addBasketProductRequest)
    {
        var customerBasket = await basketStore.GetBasketByCustomerIdAsync(customerId);

        var cachedProductPrice = decimal.Parse(await cache.GetStringAsync(addBasketProductRequest.ProductId));

        customerBasket.AddBasketProduct(new BasketProduct(
             addBasketProductRequest.ProductId,
             addBasketProductRequest.ProductName,
             cachedProductPrice,
             addBasketProductRequest.Quantity
        ));

        await basketStore.UpdateCustomerBasketAsync(customerBasket);

        return TypedResults.NoContent();
    }

    internal static async Task<IResult> DeleteBasketProduct(IBasketStore basketStore, string customerId, string productId)
    {
        var customerBasket = await basketStore.GetBasketByCustomerIdAsync(customerId);

        customerBasket.RemoveBasketProduct(productId);

        basketStore.UpdateCustomerBasketAsync(customerBasket);

        return TypedResults.NoContent();
    }

    internal static async Task<IResult> DeleteBasket(IBasketStore basketStore, string customerId)
    {
        await basketStore.DeleteCustomerBasketAsync(customerId);

        return TypedResults.NoContent();
    }
}
