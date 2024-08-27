using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public int shopping_cart_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public Product? product { get; set; }
        public ShoppingCart? shopping_cart { get; set; }
    }
}