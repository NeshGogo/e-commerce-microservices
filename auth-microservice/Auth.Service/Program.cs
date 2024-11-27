using Auth.Service.Infrastructure.Data.EntityFramework;
using Auth.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDataStore(builder.Configuration);
builder.Services.RegisterTokenService(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.MapGet("/", () => "Hello World!");

app.Run();
