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
        app.MapGet("{customerId}", async (string customerId, [FromServices] IBasketStore basketStore) =>
                await basketStore.GetBasketByCustomerIdAsync(customerId));

        app.MapPost("{customerId}", async (IBasketStore basketStore, IDistributedCache cache, string customerId,
            CreateBasketRequest createBasketRequest) =>
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
        });

        app.MapPut("{customerId}", async (IBasketStore basketStore, IDistributedCache cache, string customerId,
            AddBasketProductRequest addBasketProductRequest) =>
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
        });

        app.MapDelete("{customerId}/{productId}", async (IBasketStore basketStore, string customerId, string productId) =>
        {
            var customerBasket = await basketStore.GetBasketByCustomerIdAsync(customerId);

            customerBasket.RemoveBasketProduct(productId);

            basketStore.UpdateCustomerBasketAsync(customerBasket);

            return TypedResults.NoContent();
        });

        app.MapDelete("{customerId}", async (IBasketStore basketStore, string customerId) =>
        {
            await basketStore.DeleteCustomerBasketAsync(customerId);

            return TypedResults.NoContent();
        });
    }
}
