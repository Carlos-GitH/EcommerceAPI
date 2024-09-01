using EcommerceAPI.Data;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Middlewares;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var connectionString = builder.Configuration.GetConnectionString("RemoteConnection");

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<SellersRepository>();
builder.Services.AddScoped<ProductsRepository>();
builder.Services.AddScoped<OrdersRepository>();
builder.Services.AddScoped<ClientsRepository>();
builder.Services.AddScoped<OrderItemsRepository>();
builder.Services.AddScoped<ShoppingCartsRepository>();
builder.Services.AddScoped<CartItemsRepository>();
builder.Services.AddScoped<TokensRepository>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<ShoppingCartsService>();
builder.Services.AddScoped<ClientsService>();
builder.Services.AddScoped<CartItemsService>();
builder.Services.AddScoped<SellersService>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// app.UseMiddleware<AuthorizationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();