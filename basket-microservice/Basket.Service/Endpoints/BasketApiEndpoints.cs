using Basket.Service.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("{customerId}", 
            (string customerId, [FromServices] IBasketStore basketStore) =>
            basketStore.GetBasketByCustomerId(customerId));
    }
}
