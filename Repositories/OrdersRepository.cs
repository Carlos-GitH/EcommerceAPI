using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class OrdersRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly OrdersService _orderService;

        public OrdersRepository(AppDbContext dbContext, OrdersService orderService)
        {
            _dbContext = dbContext;
            _orderService = orderService;
        }

        public async Task<List<GetOrdersDetailedDTO>> GetAll()
        {
            var orders = await _dbContext.Orders
                                .Select(o => new GetOrdersDetailedDTO
                                {
                                    id            = o.id,
                                    client_id     = o.client_id,
                                    seller_id     = o.seller_id,
                                    delivery_type = o.delivery_type,
                                    order_status  = o.order_status,
                                    total_price   = o.total_price,
                                    client_name   = o.client.name,
                                    seller_name   = o.seller.name,
                                    order_items   = o.order_items.Select(oi => new OrderItemDTO
                                    {
                                        id            = oi.id,
                                        product_id    = oi.product_id,
                                        product_title = oi.product.title,
                                        product_price = oi.product.price,
                                        quantity      = oi.quantity

                                    }).ToList()

                                }).ToListAsync();
                                return orders;
        }

        public async Task<GetOrderDTO> GetById(int id)
        {
            var order =  _dbContext.Orders
                            .Select(o => new GetOrderDTO
                            {
                                 id            = o.id,
                                 client_id     = o.client_id,
                                 seller_id     = o.seller_id,
                                 delivery_type = o.delivery_type,
                                 order_status  = o.order_status,
                                 total_price   = o.total_price,
                                 client_name   = o.client.name,
                                 seller_name   = o.seller.name,
                                 order_items   = o.order_items.Select(oi => new OrderItemDTO
                                 {
                                     id            = oi.id,
                                     product_id    = oi.product_id,
                                     product_title = oi.product.title,
                                     product_price = oi.product.price,
                                     quantity      = oi.quantity
                                 }).ToList()
                             }).Where(o => o.id == id).FirstOrDefault();
            return order;

            
        }

        public async Task<List<GetOrdersDetailedDTO>> GetByClientId(int id)
        {
            var orders = await _dbContext.Orders
                                .Select(o => new GetOrdersDetailedDTO
                                {
                                    id            = o.id,
                                    client_id     = o.client_id,
                                    seller_id     = o.seller_id,
                                    delivery_type = o.delivery_type,
                                    order_status  = o.order_status,
                                    total_price   = o.total_price,
                                    client_name   = o.client.name,
                                    seller_name   = o.seller.name,
                                    order_items   = o.order_items.Select(oi => new OrderItemDTO
                                    {
                                        id            = oi.id,
                                        product_id    = oi.product_id,
                                        product_title = oi.product.title,
                                        product_price = oi.product.price,
                                        quantity      = oi.quantity

                                    }).ToList()

                                }).Where(o => o.client_id == id).ToListAsync();
                                return orders;
        }

        public async Task<List<GetOrdersDetailedDTO>> GetByCpfCnpj(string cpf_cnpj)
        {
            var sql = $@"SELECT o.id
                              , o.client_id
                              , o.seller_id
                              , o.delivery_type
                              , o.order_status
                              , o.total_price
                              , c.name          AS client_name
                              , s.name          AS seller_name
                           FROM orders AS o
                          INNER JOIN clients AS c
                             ON o.client_id = c.id
                          INNER JOIN sellers AS s
                             ON o.seller_id = s.id
                          WHERE c.cpf_cnpj = @cpf_cnpj;";

            var parameters = new [] { new Npgsql.NpgsqlParameter("cpf_cnpj", cpf_cnpj) };

            var orders = await _dbContext.Set<GetOrdersDetailedDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            return orders;
        }
        public async Task<CreatedOrderDTO> Create(CreateOrderDTO createOrderDTO, decimal totalPrice)
        {
            var sql = $@"INSERT INTO orders (client_id
                                           , seller_id
                                           , delivery_type
                                           , order_status
                                           , total_price)
                                VALUES (@client_id
                                      , @seller_id
                                      , @delivery_type
                                      , @order_status
                                      , @total_price)
                                      RETURNING *;";

            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("client_id", createOrderDTO.client_id),
                new Npgsql.NpgsqlParameter("seller_id", createOrderDTO.seller_id),
                new Npgsql.NpgsqlParameter("delivery_type", createOrderDTO.delivery_type),
                new Npgsql.NpgsqlParameter("order_status", createOrderDTO.order_status),
                new Npgsql.NpgsqlParameter("total_price", NpgsqlTypes.NpgsqlDbType.Numeric) {Value = totalPrice},
            };
            var order = await _dbContext.Set<CreatedOrderDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            return new CreatedOrderDTO
            {
                id = order[0].id,
                client_id = order[0].client_id,
                seller_id = order[0].seller_id,
                delivery_type = order[0].delivery_type,
                order_status = order[0].order_status,
                total_price = order[0].total_price
            };
        }

        public async Task<OrderIdDTO> UpdateOrderStatus(OrderIdDTO orderIdDTO, string order_status)
        {
            if (!Status.IsValidStatus(order_status)) return null;
            var sql = $@"UPDATE orders
                           SET order_status = @order_status
                         WHERE id = @id
                         RETURNING *;";
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("order_status", order_status),
                new Npgsql.NpgsqlParameter("id", orderIdDTO.id)
            };
            var order = await _dbContext.Set<OrderIdDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            return order[0];
        }
    }
}