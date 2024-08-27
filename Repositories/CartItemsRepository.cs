using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class CartItemsRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly CartItemsService _cartItemsService;
        public CartItemsRepository(AppDbContext dbContext, CartItemsService cartItemsService)
        {
            _dbContext = dbContext;
            _cartItemsService = cartItemsService;
        }

        public async Task<IEnumerable<CartItemDTO>> GetAll()
        {
            return await _dbContext.Set<CartItemDTO>().FromSqlRaw("SELECT * FROM shopping_cart_items;").ToListAsync();
        }

        public async Task<CartItemDTO> Create(CreateCartItemDTO cartItem)
        {
            var sql = @$" INSERT INTO shopping_cart_items (shopping_cart_id
                                                        , product_id
                                                        , quantity)
                                 VALUES (@shopping_cart_id
                                      , @product_id
                                      , @quantity)
                          RETURNING *;";
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("shopping_cart_id", cartItem.shopping_cart_id),
                new Npgsql.NpgsqlParameter("product_id", cartItem.product_id),
                new Npgsql.NpgsqlParameter("quantity", cartItem.quantity)
            };

            var createdCartItem = await _dbContext.Set<CartItemDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            if(createdCartItem.Count == 0) return null;
            return createdCartItem[0];
        }

        
        public async Task<bool> ResetShoppingCart(int id)
        {
            var sql = $"DELETE FROM shopping_cart_items WHERE shopping_cart_id = @id;";
            var parameters = new [] { new Npgsql.NpgsqlParameter("id", id) };
            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
            return true;
        }

        public async Task<bool> Remove(int id)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.id == id);
            if(cartItem is null) return false;
            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}