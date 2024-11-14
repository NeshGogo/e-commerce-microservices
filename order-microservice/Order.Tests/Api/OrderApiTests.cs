using System.Net;
using System.Net.Http.Json;
using Order.Service.ApiModels;

namespace Order.Tests.Api;

public class OrderApiTests : IntegrationTestBase
{
    public OrderApiTests(OrderWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task GetOrder_WhenNoOrderExists_ThenReturnsNotFount()
    {
        var response = await HttpClient.GetAsync($"/1/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_WhenOrderExists_ThenReturnsOrder()
    {
        var order = new Service.Models.Order { CustomerId = "1" };
        await OrderContext.CreateOrderAsync(order);

        var response = await HttpClient.GetAsync($"/{order.CustomerId}/{order.OrderId}");

        response.EnsureSuccessStatusCode();

        var getOrderResponse = await response.Content.ReadFromJsonAsync<GetOrderResponse>();

        Assert.NotNull(getOrderResponse);
        Assert.Equal(order.OrderId, getOrderResponse.OrderId);
    }
}
