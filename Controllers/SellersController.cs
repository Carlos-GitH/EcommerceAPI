using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Services;
using static EcommerceAPI.DTOs.SellerDTO;
using System.Runtime.CompilerServices;
using EcommerceAPI.Repositories;
using EcommerceAPI.Filters;

namespace EcommerceAPI.Controllers 
{
    [ApiController]
    [Route("/api/v1/sellers")]
    [LogActionFilter()]
    public class SellersController : ControllerBase
    {
        // private readonly AppDbContext _dbContext;
        private readonly SellersRepository _sellerRepository;
        private readonly TokenService _tokenService;
        public SellersController(SellersRepository sellerRepository, TokenService tokenService)
        {
            _sellerRepository = sellerRepository;
            _tokenService     = tokenService;
        }

        [HttpGet("")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetAll()
        {
            var sellers = await _sellerRepository.GetAll();
            if(sellers is null) return NotFound("Sellers not found");
            var getSellersDTO = new GetSellersDTO
            {
                sellers = sellers.Select(s => new GetSellerDTO
                {
                    id        = s.id,
                    name      = s.name,
                    reference = s.reference,
                    phone     = s.phone is null ? "" : s.phone,
                    email     = s.email
                }).ToList()
            };
            getSellersDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getSellersDTO);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> Get(int id)
        {
            var seller = await _sellerRepository.GetById(id);
            if(seller is null) return NotFound("Seller not found");
            var getSellerDTO = new GetSellerWithTokenDTO
            {
                id        = seller.id,
                name      = seller.name,
                reference = seller.reference,
                phone     = seller.phone,
                email     = seller.email
            };
            getSellerDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getSellerDTO);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> Create([FromBody] CreateSellerDTO createSellerDTO)
        {
            var seller = new Seller
            {
                name      = createSellerDTO.name,
                reference = SellersService.ReferenceBuilder(createSellerDTO.reference),
                phone     = createSellerDTO.phone,
                email     = createSellerDTO.email,
                password  = createSellerDTO.password
            };
            seller = await _sellerRepository.Create(seller);
            var createdSeller = new GetSellerWithTokenDTO
            {
                id        = seller.id,
                name      = seller.name,
                reference = seller.reference,
                phone     = seller.phone,
                email     = seller.email
            };
            createdSeller.token = await _tokenService.GenerateAndSaveToken();
            return Ok(createdSeller);
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginSellerDTO loginSellerDTO)
        {
            var seller = await _sellerRepository.GetByEmail(loginSellerDTO.email);
            if (seller is null) return NotFound("Seller not found");
            if (seller.password != loginSellerDTO.password) return BadRequest("Invalid password");
            var generatedToken = await _tokenService.GenerateAndSaveToken();
            var logguedSeller  = new LogguedSellerDTO
            {
                token = generatedToken,
                id    = seller.id,
                email = seller.email
            };
            return Ok(logguedSeller);
        }

        [HttpPost("get_by_email")]
        [Consumes("application/json")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetSellerByEmail([FromBody] GetSellerByEmailDTO getSellerByEmailDTO)
        {
            // var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.email == getSellerByEmailDTO.email);
            var seller = await _sellerRepository.GetByEmail(getSellerByEmailDTO.email);
            if (seller is null) return NotFound("Seller not found");
            var getSellerDTO = new GetSellerWithTokenDTO
            {
                id        = seller.id,
                name      = seller.name,
                reference = seller.reference,
                phone     = seller.phone,
                email     = seller.email
            };
            getSellerDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getSellerDTO);
        }

        [HttpPost("get_by_reference")]
        [Consumes("application/json")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetSellerByReference([FromBody] GetSellerByReferenceDTO getSellerByReferenceDTO)
        {
            // var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.reference == getSellerByReferenceDTO.reference);
            var seller = await _sellerRepository.GetByReference(getSellerByReferenceDTO.reference);
            if (seller is null) return NotFound("Seller not found");
            var getSellerDTO = new GetSellerWithTokenDTO
            {
                id        = seller.id,
                name      = seller.name,
                reference = seller.reference,
                phone     = seller.phone,
                email     = seller.email
            };
            getSellerDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getSellerDTO);
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSellerDTO updateSellerDTO)
        {
            // var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == id);
            var seller = await _sellerRepository.GetById(id);
            if (seller is null) return NotFound("Seller not found");
            seller.name  = updateSellerDTO.name is null ? seller.name : updateSellerDTO.name;
            seller.phone = updateSellerDTO.phone is null ? seller.phone : updateSellerDTO.phone;
            seller.email = updateSellerDTO.email is null ? seller.email : updateSellerDTO.email;
            // await _dbContext.SaveChangesAsync();
            await _sellerRepository.Update(seller);
            var getSellerDTO = new GetSellerWithTokenDTO
            {
                id        = seller.id,
                name      = seller.name,
                reference = seller.reference,
                phone     = seller.phone,
                email     = seller.email
            };
            getSellerDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getSellerDTO);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            // var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == id);
            var seller = await _sellerRepository.GetById(id);
            if (seller is null) return NotFound("Seller not found");
            // _dbContext.Sellers.Remove(seller);
            // await _dbContext.SaveChangesAsync();
            await _sellerRepository.Delete(seller);
            var token = await _tokenService.GenerateAndSaveToken();
            return Ok(token);
        }
    }
}