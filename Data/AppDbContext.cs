using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base (options){}
        public DbSet<Client> Clients { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Seller>().ToTable("sellers");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<OrderItem>().ToTable("order_items");
            modelBuilder.Entity<ShoppingCart>().ToTable("shopping_carts");
            modelBuilder.Entity<CartItem>().ToTable("shopping_cart_items");
            modelBuilder.Entity<Token>().ToTable("tokens");

            modelBuilder.Entity<GetOrdersDetailedDTO>().HasNoKey();
            modelBuilder.Entity<GetOrderDTO>().HasNoKey();
            modelBuilder.Entity<CreateOrderDTO>().HasNoKey();
            modelBuilder.Entity<CreatedOrderDTO>().HasNoKey();
            modelBuilder.Entity<CreatedOrderItemDTO>().HasNoKey();
            modelBuilder.Entity<GetOrdersByCpfCnpjDTO>().HasNoKey();
            modelBuilder.Entity<OrderIdDTO>().HasNoKey();
            modelBuilder.Entity<CreatedShoppingCartDTO>().HasNoKey();
            modelBuilder.Entity<CartItemDTO>().HasNoKey();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.order_items)
                .WithOne(oi => oi.order)
                .HasForeignKey(oi => oi.order_id);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.client)
                .WithMany(c => c.orders)
                .HasForeignKey(o => o.client_id);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.seller)
                .WithMany(s => s.orders)
                .HasForeignKey(o => o.seller_id);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.product)
                .WithMany(p => p.order_items)
                .HasForeignKey(oi => oi.product_id);
            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.cart_items)
                .WithOne(ci => ci.shopping_cart)
                .HasForeignKey(ci => ci.shopping_cart_id);
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.client)
                .WithMany(c => c.shopping_carts)
                .HasForeignKey(sc => sc.client_id);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.product)
                .WithMany(p => p.cart_items)
                .HasForeignKey(ci => ci.product_id);
        }
    }
}