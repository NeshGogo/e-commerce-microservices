using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Service.Infrastructure.Data.EntityFramework;

namespace Order.Tests;

public class OrderWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private OrderContext _orderContext;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configurationBuilder =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Appsettings.Tests.json")
                .Build();

            configurationBuilder.AddConfiguration(configuration);
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ApplyMigrations);
    }

    private void ApplyMigrations(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        _orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
        
        _orderContext.Database.Migrate();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    Task IAsyncLifetime.DisposeAsync()
    {
        if(_orderContext is not null)
        {
            return _orderContext.Database.EnsureDeletedAsync();
        }

        return Task.CompletedTask;
    }
}
