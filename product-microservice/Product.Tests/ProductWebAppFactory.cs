using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product.Service.Infrastructure.Data.EntityFramework;

namespace Product.Tests;

public class ProductWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private ProductContext _productContext;

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
        _productContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
        
        _productContext.Database.Migrate();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    Task IAsyncLifetime.DisposeAsync()
    {
        if (_productContext != null)
        {
            return _productContext.Database.EnsureDeletedAsync();
        }
        return Task.CompletedTask;
    }
}
