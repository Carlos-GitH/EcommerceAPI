using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class OrderItem
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        // [NotMapped]
        public string? product_title { get; set; }
        // [NotMapped]
        public decimal? product_price { get; set; }
        public Order? order { get; set; }
        public Product? product { get; set; }
    }
}