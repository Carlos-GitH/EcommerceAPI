using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class OrderItemsRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderItemsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int? orderId)
        {
            return await _dbContext.OrderItems.Where(o => o.order_id == orderId).ToListAsync();
        }

        public async Task<OrderItemDTO> Create(CreateOrderItemDTO createOrderItemDTO, int id)
        {
            var orderItemParameters = new []
                {
                    new Npgsql.NpgsqlParameter("order_id", id),
                    new Npgsql.NpgsqlParameter("product_id", createOrderItemDTO.product_id),
                    new Npgsql.NpgsqlParameter("quantity", createOrderItemDTO.quantity),
                };
            var sql = $@"INSERT INTO order_items (order_id
                                               , product_id
                                               , quantity) 
                              VALUES (@order_id
                                   , @product_id
                                   , @quantity)
                                   RETURNING *;";
            List<CreatedOrderItemDTO> orderItem = await _dbContext.Set<CreatedOrderItemDTO>().FromSqlRaw(sql, orderItemParameters).ToListAsync();

            var orderItemDTO = new OrderItemDTO
            {
                id = id,
                order_id = orderItem[0].order_id,
                product_id = orderItem[0].product_id,
                quantity = orderItem[0].quantity
            };
            return orderItemDTO;
        }
    }
}