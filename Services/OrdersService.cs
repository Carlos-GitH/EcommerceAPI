using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using PaymentsApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using PaymentsApi.DTOs;

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
            Product product  = await _productRepository.GetById(item.product_id);
            var reducedStock = await _productRepository.ReduceStock(product, item.quantity);
            return reducedStock;
        }

        public async Task<Product> RestoreStock(OrderItemDTO item)
        {
            Product product     = await _productRepository.GetById(item.product_id);
            var restoredStock = await _productRepository.RestoreStock(product, item.quantity);
            return restoredStock;
        }

        public decimal totalItemPrice(decimal price, int quantity)
        {
            return price * quantity;
        }

        public async Task<string> AutenticatePayment(HttpClient httpClient, string apiKey)
        {
            var apiKeyDTO = new ApiKeyDTO
            {
                api_key = apiKey
            };
            string url = "http://localhost:5087/api/v1/Autenticate";
            // httpClient.DefaultRequestHeaders.Add("api_key", apiKey.api_key);
            if (string.IsNullOrEmpty(apiKeyDTO.api_key))
            {
                return null;
            }

            var jsonContent = JsonConvert.SerializeObject(apiKeyDTO);
            var content     = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);
            if (responseBody is null)
            {
                return null;
            }
            return responseBody;
        }

        public async Task<string> Pay(HttpClient httpClient, PayDTO paymentInfo, string api_key, string token)
        {
            string url = "http://localhost:5087/api/v1/Payment/pay";
            httpClient.DefaultRequestHeaders.Remove("api_key");
            httpClient.DefaultRequestHeaders.Add("api_key", api_key);
            httpClient.DefaultRequestHeaders.Add("auth", token);

            var jsonContent = JsonConvert.SerializeObject(paymentInfo);
            var content     = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine(response.ReasonPhrase);
                return null;
            }
            var responseData = await response.Content.ReadAsStringAsync();
            return responseData;
        }
    }
}