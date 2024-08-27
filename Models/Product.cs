using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class Product
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string? image_url { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public IEnumerable<OrderItem>? order_items { get; set; }
        public IEnumerable<CartItem>? cart_items { get; set; }
    }
}