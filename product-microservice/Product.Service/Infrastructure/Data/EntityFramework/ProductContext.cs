﻿using Microsoft.EntityFrameworkCore;
using Product.Service.Models;

namespace Product.Service.Infrastructure.Data.EntityFramework;

internal class ProductContext : DbContext, IProductStore
{
    public ProductContext(DbContextOptions options) 
        : base(options)
    {        
    }

    public DbSet<Models.Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }

    public async Task CreateProduct(Models.Product product)
    {
        Products.Add(product);

        await SaveChangesAsync();
    }

    public async Task<Models.Product> GetByIdAsync(int id) => await FindAsync<Models.Product>(id);

    public async Task UpdateProduct(Models.Product product)
    {
        var existingProduct = await FindAsync<Models.Product>(product.Id);
        if (existingProduct != null) 
        {
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            await SaveChangesAsync();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<Models.Product>(new ProductConfiguration());
        modelBuilder.ApplyConfiguration<ProductType>(new ProductTypeConfiguration());
    }
}