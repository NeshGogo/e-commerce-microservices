using Microsoft.EntityFrameworkCore;
using Product.Service.ApiModels;
using Product.Service.Infrastructure.Data.EntityFramework;
using Product.Service.IntegrationEvents;
using Product.Service.Models;
using System.Net;
using System.Net.Http.Json;

namespace Product.Tests.Api;

public class ProductApiTests : IntegrationTestBase
{
    public ProductApiTests(ProductWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task GetProduct_WhenNoProductExists_ThenReturnsNotFound()
    {
        var response = await HttpClient.GetAsync("/1");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ThenReturnsProduct()
    {
        var product = new Service.Models.Product
        {
            Name = "Test",
            Price = 1,
            ProductTypeId = 1,
            ProductType = new ProductType { Type = "Test" },
        };

        await ProductContext.CreateProduct(product);

        var response = await HttpClient.GetAsync("/1");

        response.EnsureSuccessStatusCode();

        var getProductResponse = await response.Content.ReadFromJsonAsync<GetProductResponse>();

        Assert.NotNull(getProductResponse);
        Assert.Equal(product.Id, getProductResponse.Id);
        Assert.Equal(product.Name, getProductResponse.Name);
    }

    [Fact]
    public async Task CreateProduct_WhenCalled_ThenCreatesProduct()
    {
        ProductContext.Set<ProductType>().Add(new ProductType { Type = "Test" });
        await ProductContext.SaveChangesAsync();

        var createProductRequest = new CreateProductRequest("Test", 1, 1, "Test"); 

        var response = await HttpClient.PostAsJsonAsync("", createProductRequest);

        response.EnsureSuccessStatusCode();

        var locationHeader = response.Headers.Location;

        Assert.NotNull(locationHeader);

        var productId = int.Parse(locationHeader.ToString());

        var product = ProductContext.Products.FirstOrDefault(p => p.Id == productId);

        Assert.NotNull(product);
        Assert.Equal(product.Name, createProductRequest.Name);
        Assert.Equal(product.Price, createProductRequest.Price);
        Assert.Equal(product.Description, createProductRequest.Description);
    }


    [Fact]
    public async Task UpdateProduct_WhenCalled_ThenUpdatesProduct()
    {
        var product = new Service.Models.Product
        {
            Name = "Test",
            Price = 1,
            ProductTypeId = 0,
            ProductType = new ProductType { Type = "Test" },
        };

        await ProductContext.CreateProduct(product);

        var updateProductRequest = new UpdateProductRequest("Test 2", 1, product.ProductTypeId, null);

        var response = await HttpClient.PutAsJsonAsync($"/{product.Id}", updateProductRequest);

        response.EnsureSuccessStatusCode();

        var productDb =  await ProductContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id);

        Assert.NotNull(productDb);
        Assert.Equal(updateProductRequest.Name, productDb.Name);
        Assert.Equal(updateProductRequest.Price, productDb.Price);
        Assert.Equal(updateProductRequest.Description, productDb.Description);
    }

    [Fact]
    public async Task UpdateProduct_WhenCalledWithNewPrice_ThenProductPriceUpdatedEventPublished()
    {
        var product = new Service.Models.Product
        {
            Name = "Test",
            Price = 1,
            ProductTypeId = 1,
            ProductType = new ProductType { Type = "Test" },
        };
        
        await ProductContext.CreateProduct(product);

        Subscribe<ProductPriceUpdatedEvent>();
        var updateProductRequest = new UpdateProductRequest("Test 2", 200, product.ProductTypeId, null);

        var response = await HttpClient.PutAsJsonAsync($"/{product.Id}", updateProductRequest);

        response.EnsureSuccessStatusCode();
        
        Assert.NotEmpty(ReceivedEvents);

        var receivedEvent = ReceivedEvents.First();

        Assert.IsType<ProductPriceUpdatedEvent>(receivedEvent);
        Assert.Equal((receivedEvent as ProductPriceUpdatedEvent).ProductId, product.Id);
    }
}
