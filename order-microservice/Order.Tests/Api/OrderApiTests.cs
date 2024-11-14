using System.Net;

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
}
