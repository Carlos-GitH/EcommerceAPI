using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EcommerceAPI.Repositories
{
    public class ProductsRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<Product> Create(CreateProductDTO createProductDTO)
        {
            var sql = $@"INSERT INTO products (title
                                             , description
                                             , image_url
                                             , price
                                             , stock)
                                VALUES(@title
                                     , @description
                                     , @image_url
                                     , @price
                                     , @stock)
                                RETURNING *";
            var parameters = new [] { new Npgsql.NpgsqlParameter("title", createProductDTO.title),
                                      new Npgsql.NpgsqlParameter("description", createProductDTO.description),
                                      new Npgsql.NpgsqlParameter("image_url", createProductDTO.image_url is null ? "" : createProductDTO.image_url),
                                      new Npgsql.NpgsqlParameter("price", createProductDTO.price),
                                      new Npgsql.NpgsqlParameter("stock", createProductDTO.stock) };
            Product productCreated = await _dbContext.Products.FromSqlRaw(sql, parameters).FirstOrDefaultAsync();
            return productCreated;
        }

        public async Task<Product> Update(Product product, UpdateProductDTO updateProductDTO)
        {
            var sql = $@"UPDATE products
                            SET title = @title
                              , description = @description
                              , image_url = @image_url
                              , price = @price
                              , stock = @stock
                          WHERE id = @id
                          RETURNING *;";
            var parameters = new [] { new Npgsql.NpgsqlParameter("title", updateProductDTO.title is null ? product.title : updateProductDTO.title),
                                      new Npgsql.NpgsqlParameter("description", updateProductDTO.description is null ? product.description : updateProductDTO.description),
                                      new Npgsql.NpgsqlParameter("image_url", updateProductDTO.image_url is null ? product.image_url : updateProductDTO.image_url),
                                      new Npgsql.NpgsqlParameter("price", updateProductDTO.price is null ? product.price : updateProductDTO.price),
                                      new Npgsql.NpgsqlParameter("stock", updateProductDTO.stock is null ? product.stock : updateProductDTO.stock),
                                      new Npgsql.NpgsqlParameter("id", product.id) };
            List<Product> productUpdated = await _dbContext.Products.FromSqlRaw(sql, parameters).ToListAsync();
            return productUpdated[0];
        }

        public async Task<Product> Delete(string sql, Npgsql.NpgsqlParameter[] parameters)
        {
            Product productDeleted = await _dbContext.Products.FromSqlRaw(sql, parameters).FirstOrDefaultAsync();
            await _dbContext.SaveChangesAsync();
            return productDeleted;
        }

        public async Task<Product> ReduceStock(Product product, int quantity)
        {
            UpdateProductDTO updateProductDTO = new UpdateProductDTO
            {
                stock = product.stock - quantity
            };
            var productUpdated = await Update(product, updateProductDTO);
            return productUpdated;
        }

        public async Task<Product> RestoreStock(Product product, int quantity)
        {
            UpdateProductDTO updateProductDTO = new UpdateProductDTO
            {
                stock = product.stock + quantity
            };
            var productUpdated = await Update(product, updateProductDTO);
            return productUpdated;
        }
    }
}