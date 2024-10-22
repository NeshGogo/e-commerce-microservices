using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketStore, InMemoryBasketStore>();

var app = builder.Build();

app.RegisterEndpoints();

app.Run();
