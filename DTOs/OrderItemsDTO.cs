using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class OrderItemDTO
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string? product_title { get; set; }
        public decimal product_price { get; set; }
    }
    
    public class OrderItemsDTO
    {
        public List<OrderItemDTO> orderItems { get; set; }
    }

    public class CreateOrderItemDTO
    {
        public int quantity { get; set; }
        public int product_id { get; set; }
        public int order_id { get; set; }
    }
    public class CreatedOrderItemDTO
    {
        public int quantity { get; set; }
        public int product_id { get; set; }
        public int order_id { get; set; }
        public int id { get; set; }
    }
}