using Basket.Service.ApiModels;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("{customerId}",
            (string customerId, [FromServices] IBasketStore basketStore) =>
            basketStore.GetBasketByCustomerId(customerId));

        app.MapPost("{customerId}", (IBasketStore basketStore, string customerId,
            CreateBasketRequest createBasketRequest) =>
        {
            var customerBasket = new CustomerBasket { CustomerId = customerId };

            customerBasket.AddBasketProduct(new BasketProduct(
                 createBasketRequest.ProductId,
                 createBasketRequest.ProductName
            ));

            basketStore.CreateCustomerBasket(customerBasket);

            return TypedResults.Created();
        });

        app.MapPut("{customerId}", (IBasketStore basketStore, string customerId,
            AddBasketProductRequest addBasketProductRequest) =>
        {
            var customerBasket = basketStore.GetBasketByCustomerId(customerId);

            customerBasket.AddBasketProduct(new BasketProduct(
                 addBasketProductRequest.ProductId,
                 addBasketProductRequest.ProductName,
                 addBasketProductRequest.Quantity
            ));

            basketStore.UpdateCustomerBasket(customerBasket);

            return TypedResults.NoContent();
        });
    }
}
