using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class ShoppingCart
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public decimal total_price {get; set; }
        public IEnumerable<CartItem>? cart_items { get; set; }
        public Client? client { get; set; }
    }
}