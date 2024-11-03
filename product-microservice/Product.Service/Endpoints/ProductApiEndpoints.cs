using Product.Service.Infrastructure.Data;

namespace Product.Service.Endpoints;

public static class ProductApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("{productId}", async Task<IResult> (IProductStore productStore, int productId) =>
        {
            var product = await productStore.GetByIdAsync (productId);

            return product is null 
                ? TypedResults.NotFound("Product not found")
                : TypedResults.Ok(product);
        });
    }
}
