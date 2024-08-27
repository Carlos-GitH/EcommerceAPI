using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;

namespace EcommerceAPI.Services
{
    public class ShoppingCartsService
    {
        private readonly ClientsRepository _clientRepository;
        private readonly CartItemsRepository _cartItemRepository;
        private readonly ProductsRepository _productRepository;
        private readonly OrdersRepository _orderRepository;
        private readonly OrderItemsRepository _orderItemRepository;


        public ShoppingCartsService(ClientsRepository clientRepository, CartItemsRepository cartItemRepository, ProductsRepository productRepository, OrdersRepository orderRepository, OrderItemsRepository orderItemRepository)
        {
            _clientRepository = clientRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<bool> CheckClient(int id)
        {
            if(await _clientRepository.GetById(id) is null) return false;
            return true;
        }

        public async Task<CartItemDTO> CreateCartItem(CreateCartItemDTO cartItem)
        {
            var createdCartItem = await _cartItemRepository.Create(cartItem);
            return createdCartItem;
        }

        public async Task<bool> ResetShoppingCart(int id)
        {
            return await _cartItemRepository.ResetShoppingCart(id);
        }

        public async Task<decimal> VerifyStock(int id, int quantity)
        {
            var product = await _productRepository.GetById(id);
            if(product is null) return 0;
            if(product.stock < quantity) return -1;
            return product.price;
        }

        public async Task<Product> ReduceStock(int id, int quantity)
        {
            var product = await _productRepository.GetById(id);
            await _productRepository.ReduceStock(product, quantity);
            return product;
        }

        public async Task<Product> RestoreStock(int id, int quantity)
        {
            Product product = await _productRepository.GetById(id);
            var restoredStock = await _productRepository.RestoreStock(product, quantity);
            return restoredStock;
        }

        public decimal TotalItemPrice(decimal price, int quantity)
        {
            return price * quantity;
        }

        public async Task<bool> RemoveCartItem(int id)
        {
            return await _cartItemRepository.Remove(id);
        }

        public async Task<dynamic> BuyShoppingCart(ShoppingCartDTO shoppingCart, string deliveryType)
        {
            var orderToCreate = new CreateOrderDTO
            {
                client_id   = shoppingCart.client_id,
                seller_id   = 77,
                delivery_type = deliveryType
            };
            var createdOrder = await _orderRepository.Create(orderToCreate, shoppingCart.total_price);
            foreach (var item in shoppingCart.cart_items)
            {
                var orderItemToCreate = new CreateOrderItemDTO
                {
                    order_id    = createdOrder.id,
                    product_id  = item.product_id,
                    quantity    = item.quantity
                };
                await CreateOrderItem(orderItemToCreate, createdOrder.id);
            }
            var orderToReturn = await _orderRepository.GetById(createdOrder.id);
            return orderToReturn;

        }

        public async Task<CreateOrderItemDTO> CreateOrderItem(CreateOrderItemDTO createOrderItemDTO, int id)
        {
            await _orderItemRepository.Create(createOrderItemDTO, id);
            return createOrderItemDTO;
        }
    }
}