using Microsoft.Extensions.DependencyInjection;
using Order.Service.Infrastructure.Data.EntityFramework;

namespace Order.Tests;

public class IntegrationTestBase : IClassFixture<OrderWebApplicationFactory>
{
    public IntegrationTestBase(OrderWebApplicationFactory webApplicationFactory)
    {
        var scope = webApplicationFactory.Services.CreateScope();

        OrderContext = scope.ServiceProvider.GetService<OrderContext>();
        HttpClient = webApplicationFactory.CreateClient();
        
    }

    internal readonly OrderContext? OrderContext;
    internal readonly HttpClient HttpClient;
}
