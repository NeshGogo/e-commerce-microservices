﻿using Microsoft.EntityFrameworkCore;

namespace Product.Service.Infrastructure.Data.EntityFramework;

public static class EntityFrameworkExtensions
{
    public static void AddSqlServerDatastore(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddDbContext<ProductContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("Default")));
        
        services.AddScoped<IProductStore, ProductContext>();
    }
}