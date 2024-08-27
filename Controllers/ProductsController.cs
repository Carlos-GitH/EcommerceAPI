using EcommerceAPI.DTOs;
using EcommerceAPI.Filters;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/products")]
    [LogActionFilter()]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsRepository _productRepository;
        private readonly TokenService _tokenService;
        public ProductsController(ProductsRepository productRepository, TokenService tokenService)
        {
            _productRepository = productRepository;
            _tokenService      = tokenService;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            // var products = await _dbContext.Products.FromSqlRaw("SELECT * FROM products").ToListAsync();
            var products = await _productRepository.GetAll();
            if (products is null) return NotFound("Products not found");
            var getProductsDTO = new GetProductsDTO
            {
                products = products.Select(p => new GetProductDTO
                {
                    id          = p.id,
                    title       = p.title,
                    description = p.description,
                    image_url   = p.image_url,
                    price       = p.price,
                    stock       = p.stock
                }).ToList()
            };
            getProductsDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getProductsDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            // var product = await _dbContext.Products.FromSqlInterpolated($"SELECT * FROM products WHERE id = {id}").ToListAsync();
            var product = await _productRepository.GetById(id);
            if (product is null) return NotFound("Product not found");
            var getProductDTO = new GetProductWithTokenDTO
            {
                id          = product.id,
                title       = product.title,
                description = product.description,
                image_url   = product.image_url,
                price       = product.price,
                stock       = product.stock
            };
            getProductDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getProductDTO);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            var productCreated = await _productRepository.Create(createProductDTO);
            var productToReturn = new GetProductWithTokenDTO
            {
                id          = productCreated.id,
                title       = productCreated.title,
                description = productCreated.description,
                image_url   = productCreated.image_url,
                price       = productCreated.price,
                stock       = productCreated.stock
            };
            productToReturn.token = await _tokenService.GenerateAndSaveToken();
            return Ok(productToReturn);
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO updateProductDTO)
        {
            // var product = await _dbContext.Products.FromSqlInterpolated($"SELECT * FROM products WHERE id = {id}").ToListAsync();
            var product = await _productRepository.GetById(id);
            if (product is null) return NotFound("Product not found");
            // var product_updated = _dbContext.Products.FromSqlRaw(sql, parameters).FirstOrDefaultAsync();
            _productRepository.Update(product, updateProductDTO);
            var productReturn = new GetProductWithTokenDTO
            {
                id          = id,
                title       = updateProductDTO.title is null ? product.title : updateProductDTO.title,
                description = updateProductDTO.description is null ? product.description : updateProductDTO.description,
                image_url   = updateProductDTO.image_url is null ? product.image_url : updateProductDTO.image_url,
                price       = (decimal)updateProductDTO.price,
                stock       = (int)updateProductDTO.stock
            };
            productReturn.token = await _tokenService.GenerateAndSaveToken();
            return Ok(productReturn);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> DeleteProduct(int id)
        { 
            var sql = $@"DELETE FROM products WHERE id = @id RETURNING *;";
            var parameters = new [] { new Npgsql.NpgsqlParameter("id", id) };
            // var product_deleted = _dbContext.Products.FromSqlRaw(sql, parameters).AsEnumerable();
            var productDeleted = await _productRepository.Delete(sql, parameters);
            if (productDeleted is null) return NotFound("Product not found");
            var productReturn = new GetProductWithTokenDTO
            {
                id          = id,
                title       = productDeleted.title,
                description = productDeleted.description,
                image_url   = productDeleted.image_url,
                price       = productDeleted.price,
                stock       = productDeleted.stock
            };
            productReturn.token = await _tokenService.GenerateAndSaveToken();
            return Ok(productReturn);
        }
    }
}