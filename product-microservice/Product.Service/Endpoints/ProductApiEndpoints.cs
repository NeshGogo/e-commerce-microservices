using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Product.Service.ApiModels;
using Product.Service.Infrastructure.Data;
using Product.Service.IntegrationEvents;

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
                : TypedResults.Ok(new GetProductResponse(
                    product.Id,
                    product.Name, 
                    product.Price, 
                    product.ProductType?.Type, 
                    product.Description));
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

        endpoint.MapPut("{productId}", async Task<IResult> (IProductStore productStore, IEventBus eventBus,
            int productId, UpdateProductRequest request) =>
        {
            var product = await productStore.GetByIdAsync(productId);

            if (product is null)
                return TypedResults.NotFound($"Product with id {productId} does not exist");

            var existingPrice = product.Price;

            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description;
            product.ProductTypeId = request.ProductTypeId;

            await productStore.UpdateProduct(product);

            if(!decimal.Equals(existingPrice, request.Price))
            {
                await eventBus.PublishAsync(new ProductPriceUpdatedEvent(productId, request.Price));
            }

            return TypedResults.NoContent();
        });
    }
}
