using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class ShoppingCartsRepository
    {
        private readonly AppDbContext _dbContext;

        public ShoppingCartsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ShoppingCartDTO>> GetShoppingCarts()
        {
            var shoppingCarts = await _dbContext.ShoppingCarts
                                       .Select(sc => new ShoppingCartDTO
                                       {
                                            id = sc.id,
                                            client_id = sc.client_id,
                                            total_price = sc.total_price,
                                            cart_items = sc.cart_items.Select(ci => new CartItemDTO
                                            {
                                                id = ci.id,
                                                shopping_cart_id = sc.id,
                                                product_id = ci.product_id,
                                                quantity = ci.quantity
                                            }).Where(ci => ci.shopping_cart_id == sc.id).ToList()
                                       }).ToListAsync();
            return shoppingCarts;
        }

        public async Task<ShoppingCartDTO> GetById(int id)
        {
            var shoppingCart = await _dbContext.ShoppingCarts
                                       .Select(sc => new ShoppingCartDTO
                                       {
                                            id = sc.id,
                                            client_id = sc.client_id,
                                            total_price = sc.total_price,
                                            cart_items = sc.cart_items.Select(ci => new CartItemDTO
                                            {
                                                id = ci.id,
                                                shopping_cart_id = sc.id,
                                                product_id = ci.product_id,
                                                quantity = ci.quantity
                                            }).ToList()
                                       }).Where(sc => sc.id == id).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCartDTO> GetByClientId(int id)
        {
            var shoppingCart = await _dbContext.ShoppingCarts
                                       .Select(sc => new ShoppingCartDTO
                                       {
                                            id = sc.id,
                                            client_id = sc.client_id,
                                            total_price = sc.total_price,
                                            cart_items = sc.cart_items.Select(ci => new CartItemDTO
                                            {
                                                id = ci.id,
                                                shopping_cart_id = ci.id,
                                                product_id = ci.product_id,
                                                quantity = ci.quantity
                                            }).ToList()
                                       }).Where(sc => sc.client_id == id).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<CreatedShoppingCartDTO> CreateShoppingCart(CreateShoppingCartDTO shoppingCart)
        {
            var sql = $@"INSERT INTO shopping_carts 
                                     (client_id
                                   , total_price) 
                                VALUES (@client_id
                                     , 0) 
                         RETURNING *;";
            var parameters = new [] { new Npgsql.NpgsqlParameter("client_id", shoppingCart.client_id) };
            var shoppingCartReturn = await _dbContext.Set<CreatedShoppingCartDTO>().FromSqlRaw(sql, parameters).ToListAsync();

            var createdShoppingCart = shoppingCartReturn[0];

            return createdShoppingCart;
        }

        public async Task<bool> UpdateTotalPrice(ShoppingCartDTO shoppingCart, decimal totalPrice)
        {
            var updatedPrice = shoppingCart.total_price + totalPrice;
            var sql = $@"UPDATE shopping_carts SET total_price = @total_price WHERE id = @id RETURNING *;";
            var parameters = new [] 
            { 
                new Npgsql.NpgsqlParameter("total_price",NpgsqlTypes.NpgsqlDbType.Numeric) {Value = updatedPrice},
                new Npgsql.NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Numeric) {Value = shoppingCart.id}
            };
            var result = await _dbContext.ShoppingCarts.FromSqlRaw(sql, parameters).ToListAsync();
            if(result.Count == 0) return false;
            return true;
        }
    }
}