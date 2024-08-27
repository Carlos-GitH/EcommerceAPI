using EcommerceAPI.DTOs;
using EcommerceAPI.Filters;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/cart_items")]
    [LogActionFilter()]
    [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
    [TypeFilter(typeof(SellerAuthorizeActionFilter))]
    public class CartItemsController : ControllerBase
    {
        private readonly CartItemsRepository _cartItemsRepository;
        private readonly CartItemsService _cartItemsService;
        private readonly TokenService _tokenService;
        public CartItemsController(CartItemsRepository cartItemsRepository, CartItemsService cartItemsService, TokenService tokenService)
        {
            _cartItemsRepository = cartItemsRepository;
            _cartItemsService    = cartItemsService;
            _tokenService        = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartItemDTO>>> Get()
        {
            var cartItems    = await _cartItemsRepository.GetAll();
            var cartItemsDTO = new ListCartItemsWithTokenDTO
            {
                cartItems = (List<CartItemDTO>)cartItems
            };
            cartItemsDTO.token  = await _tokenService.GenerateAndSaveToken();
            return Ok(cartItemsDTO);
        }
    }
}