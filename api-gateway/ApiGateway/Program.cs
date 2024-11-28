using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ECommerce.Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", false, false);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseJwtAuthentication();

await app.UseOcelot();

app.Run();
