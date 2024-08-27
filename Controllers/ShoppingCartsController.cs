using EcommerceAPI.DTOs;
using EcommerceAPI.Filters;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/shoppingcarts")]
    [LogActionFilter()]
    [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
    public class ShoppingCartsController: ControllerBase
    {
        private readonly ShoppingCartsRepository _shoppingCartsRepository;
        private readonly ShoppingCartsService _shoppingCartsService;
        private readonly TokenService _tokenService;
        private readonly ClientsService _clientService;

        public ShoppingCartsController(ShoppingCartsRepository shoppingCartsRepository, ShoppingCartsService shoppingCartsService, TokenService tokenService, ClientsService clientService)
        {
            _shoppingCartsRepository = shoppingCartsRepository;
            _shoppingCartsService    = shoppingCartsService;
            _tokenService            = tokenService;
            _clientService           = clientService;
        }

        [HttpGet]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<ActionResult<ShoppingCartsDTO>> GetShoppingCarts()
        {
            var shoppingCarts       = await _shoppingCartsRepository.GetShoppingCarts();
            var shoppingCartsReturn = new ShoppingCartsDTO
            {
                shopping_carts = shoppingCarts
            };
            // shoppingCartsReturn.token = await _tokenService.GenerateAndSaveToken();
            return Ok(shoppingCartsReturn);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<ActionResult<CreateShoppingCartDTO>> CreateShoppingCart([FromBody] CreateShoppingCartDTO shoppingCart)
        {
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, shoppingCart.client_id)) return Unauthorized("Unauthorized");
            }
            bool checkClient = await _shoppingCartsService.CheckClient(shoppingCart.client_id);
            if(!checkClient) return BadRequest("Client not found");
            var checkCarrinho = await _shoppingCartsRepository.GetByClientId(shoppingCart.client_id);
            if(checkCarrinho is not null) return BadRequest("Shopping cart already exists");
            var createdShoppingCart   = await _shoppingCartsRepository.CreateShoppingCart(shoppingCart);
            createdShoppingCart.token = await _tokenService.GenerateAndSaveToken();
            return Ok(createdShoppingCart);
        }

        [HttpPost("add_to_shopping_cart")]
        [Consumes("application/json")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<ActionResult<List<CreateCartItemDTO>>> AddToShoppingCart([FromBody] CreateCartItemsDTO cartItem)
        {
            decimal total = 0;
            var shoppingCart = await _shoppingCartsRepository.GetById(cartItem.cartItems[0].shopping_cart_id);
            if(shoppingCart is null) return BadRequest("Shopping cart not found");

            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, shoppingCart.client_id)) return Unauthorized("Unauthorized");
            }

            foreach(var item in cartItem.cartItems)
            {

                var price = await _shoppingCartsService.VerifyStock(item.product_id, item.quantity);
                if(price == 0) return BadRequest($"Product {item.product_id} not found");
                if(price == -1) return BadRequest($"Product {item.product_id} out of stock");
                total += _shoppingCartsService.TotalItemPrice(price, item.quantity);
            }

            CreatedCartItemsDTO createdCartItems = new CreatedCartItemsDTO();
            createdCartItems.cartItems = new List<CartItemDTO>();

            foreach(var item in cartItem.cartItems)
            {
                var createdCartItem = await _shoppingCartsService.CreateCartItem(item);
                await _shoppingCartsService.ReduceStock(item.product_id, item.quantity);
                var result = await _shoppingCartsRepository.UpdateTotalPrice(shoppingCart, total);

                createdCartItems.cartItems.Add(createdCartItem);
            }

            createdCartItems.token = await _tokenService.GenerateAndSaveToken();
            return Ok(createdCartItems);
        }

        [HttpPost("remove_from_shopping_cart/{id}")]
        [Consumes("application/json")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<ActionResult<List<CartItemDTO>>> RemoveFromShoppingCart([FromBody] RemoveCartItemFromShoppingCartDTO cartItem, int id)
        {
            decimal totalToSubtract = 0;
            var shoppingCart = await _shoppingCartsRepository.GetById(id);
            if(shoppingCart is null) return BadRequest("Shopping cart not found");

            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, shoppingCart.client_id)) return Unauthorized("Unauthorized");
            }

            if(shoppingCart.cart_items.Count() == 0 || shoppingCart.cart_items is null) return BadRequest("Cart is empty");
            ListCartItemsWithTokenDTO updatedCartItems = new ListCartItemsWithTokenDTO();
            foreach(var item in shoppingCart.cart_items) 
            {
                if(item.id != cartItem.id) updatedCartItems.cartItems.Add(item);
                else {
                    await _shoppingCartsService.RemoveCartItem(cartItem.id);
                    var product      = await _shoppingCartsService.RestoreStock(item.product_id, item.quantity);
                    totalToSubtract -= _shoppingCartsService.TotalItemPrice(product.price, item.quantity);
                }
            }
            await _shoppingCartsRepository.UpdateTotalPrice(shoppingCart, totalToSubtract);
            var token = await _tokenService.GenerateAndSaveToken();
            updatedCartItems.token = token;
            return Ok(updatedCartItems);
        }

        [HttpGet("reset_shopping_cart/{id}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<ActionResult> ResetShoppingCart(int id)
        {
            var shoppingCart = await _shoppingCartsRepository.GetById(id);
            if(shoppingCart is null) return BadRequest("Shopping cart not found");

            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, shoppingCart.client_id)) return Unauthorized("Unauthorized");
            }

            if(shoppingCart.cart_items.Count == 0) return BadRequest("Cart is empty");
            await _shoppingCartsService.ResetShoppingCart(id);
            var shoppingCartClean = await _shoppingCartsRepository.GetById(id);
            var totalToSubtract   = - shoppingCart.total_price;
            await _shoppingCartsRepository.UpdateTotalPrice(shoppingCart, totalToSubtract);
            string token = await _tokenService.GenerateAndSaveToken();
            return Ok(token);
        }

        [HttpGet("buy_shopping_cart/{id}/{deliveryType}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        public async Task<ActionResult> BuyShoppingCart(int id, string deliveryType)
        {
            var shoppingCart = await _shoppingCartsRepository.GetById(id);

            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, shoppingCart.client_id)) return Unauthorized("Unauthorized");
            }
            
            if(shoppingCart is null) return BadRequest("Shopping cart not found");
            if(shoppingCart.cart_items is null) return BadRequest("Cart is empty");
            if(shoppingCart.cart_items.Count == 0) return BadRequest("Cart is empty");
            var order       = await _shoppingCartsService.BuyShoppingCart(shoppingCart, deliveryType);
            var orderReturn = new GetOrderWithTokenDTO
            {
                id     = order.id,
                client_id     = order.client_id,
                seller_id     = order.seller_id,
                delivery_type = order.delivery_type,
                order_status  = order.order_status,
                total_price   = order.total_price,
                client_name   = order.client_name,
                seller_name   = order.seller_name,
                order_items   = order.order_items
            };
            var token = await _tokenService.GenerateAndSaveToken();
            orderReturn.token = token;
            return Ok(orderReturn);
        }
    }
}