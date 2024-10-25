using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderStore, InMemoryOrderStore>();

var app = builder.Build();

app.RegisterEndpoints();

app.Run();
