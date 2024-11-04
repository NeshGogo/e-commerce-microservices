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
        app.MapGet("{customerId}",
            (string customerId, [FromServices] IBasketStore basketStore) =>
            basketStore.GetBasketByCustomerId(customerId));

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

            basketStore.CreateCustomerBasket(customerBasket);

            return TypedResults.Created();
        });

        app.MapPut("{customerId}", async (IBasketStore basketStore, IDistributedCache cache, string customerId,
            AddBasketProductRequest addBasketProductRequest) =>
        {
            var customerBasket = basketStore.GetBasketByCustomerId(customerId);

            var cachedProductPrice = decimal.Parse(await cache.GetStringAsync(addBasketProductRequest.ProductId));

            customerBasket.AddBasketProduct(new BasketProduct(
                 addBasketProductRequest.ProductId,
                 addBasketProductRequest.ProductName,
                 cachedProductPrice,
                 addBasketProductRequest.Quantity
            ));

            basketStore.UpdateCustomerBasket(customerBasket);

            return TypedResults.NoContent();
        });

        app.MapDelete("{customerId}/{productId}", (IBasketStore basketStore, string customerId, string productId) =>
        {
            var customerBasket = basketStore.GetBasketByCustomerId(customerId);

            customerBasket.RemoveBasketProduct(productId);

            basketStore.UpdateCustomerBasket(customerBasket);

            return TypedResults.NoContent();
        });

        app.MapDelete("{customerId}", (IBasketStore basketStore, string customerId) =>
        {
            basketStore.DeleteCustomerBasket(customerId);

            return TypedResults.NoContent();
        });
    }
}
