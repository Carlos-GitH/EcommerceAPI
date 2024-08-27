using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;

namespace EcommerceAPI.Services
{
    public class OrdersService
    {
        private readonly ClientsRepository _clientRepository;
        private readonly SellersRepository _sellerRepository;
        private readonly ProductsRepository _productRepository;
        private readonly OrderItemsRepository _orderItemRepository;

        public OrdersService(ClientsRepository clientRepository, SellersRepository sellerRepository, ProductsRepository productRepository, OrderItemsRepository orderItemRepository)
        {
            _clientRepository = clientRepository;
            _sellerRepository = sellerRepository;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _clientRepository.GetById(id);
        }

        public async Task<Seller> GetSellerById(int id)
        {
            return await _sellerRepository.GetById(id);
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int? orderId)
        {
            return await _orderItemRepository.GetOrderItemsByOrderId(orderId);
        }
        
        public async Task<CreateOrderItemDTO> CreateOrderItem(CreateOrderItemDTO createOrderItemDTO, int id)
        {
            await _orderItemRepository.Create(createOrderItemDTO, id);
            return createOrderItemDTO;
        }

        public async Task<decimal> VerifyStock(OrderItemDTO item)
        {
            Product product = await _productRepository.GetById(item.product_id);
            if(product is null) return -1;
            if(product.stock < item.quantity) return 0;
            return product.price;
        }

        public async Task<Product> ReduceStock(OrderItemDTO item)
        {
            Product product = await _productRepository.GetById(item.product_id);
            var reducedStock = await _productRepository.ReduceStock(product, item.quantity);
            return reducedStock;
        }

        public async Task<Product> RestoreStock(OrderItemDTO item)
        {
            Product product = await _productRepository.GetById(item.product_id);
            var restoredStock = await _productRepository.RestoreStock(product, item.quantity);
            return restoredStock;
        }

        public decimal totalItemPrice(decimal price, int quantity)
        {
            return price * quantity;
        }

        

        // public async Task<Product> ReduceStock(Product product, int quantity)
        // {
        //    var productUpdated = await _productRepository.ReduceStock(product, quantity);
        //    return productUpdated;
        // }

        // public async Task<Product> GetProductById(int id)
        // {
        //     return await _productRepository.GetById(id);
        // }
    }
}