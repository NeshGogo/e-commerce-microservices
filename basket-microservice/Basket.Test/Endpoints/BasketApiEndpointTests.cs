using Basket.Service.ApiModels;
using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using System.Text;

namespace Basket.Test.Endpoints;

public class BasketApiEndpointTests
{
    private readonly IBasketStore _basketStore;
    private readonly IDistributedCache _cache;

    public BasketApiEndpointTests()
    {
        _basketStore = Substitute.For<IBasketStore>();
        _cache = Substitute.For<IDistributedCache>();
    }

    [Fact]
    public async Task GivenExistingBasket_WhenCallingGetBasket_ThenReturnsBasket()
    {
        // Arrange
        const string customerId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };
        _basketStore.GetBasketByCustomerIdAsync(customerId)
            .Returns(customerBasket);
        // Act
        var result = await BasketApiEndpoints.GetBasket(customerId, _basketStore);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
    }

    [Fact]
    public async Task GivenNewBasketRequest_WhenCallingCreateBasket_ThenReturnsCreatedResult()
    {
        // Arrange
        const string customerId = "1";
        const string productId = "1";
        var createBasketRequest = new CreateBasketRequest(productId, "Test Name");
        _cache.GetAsync(productId)
            .Returns(Encoding.UTF8.GetBytes("1.00"));
        // Act
        var result = await BasketApiEndpoints.CreateBasket(_basketStore, _cache,
            customerId, createBasketRequest);
        // Assert
        Assert.NotNull(result);
        var createdResult = (Created)result;
        Assert.NotNull(createdResult);
    }
}
