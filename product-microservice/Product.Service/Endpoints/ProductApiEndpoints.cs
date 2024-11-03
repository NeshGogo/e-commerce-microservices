using Product.Service.ApiModels;
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

        endpoint.MapPost("", async (IProductStore productStore, CreateProductRequest request) =>
        {
            var product = new Models.Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                ProductTypeId = request.ProductTypeId
            };

            await productStore.CreateProduct(product);

            return TypedResults.Created(product.Id.ToString());
        });
    }
}
