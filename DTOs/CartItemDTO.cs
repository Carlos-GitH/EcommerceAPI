using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class CartItemDTO
    {
        public int id { get; set; }
        public int shopping_cart_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
    }

    public class CreateCartItemDTO
    {
        public int shopping_cart_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
    }

    public class CreateCartItemsDTO
    {
        public  List<CreateCartItemDTO> cartItems { get; set; }
    }

    public class CreatedCartItemsDTO
    {
        public List<CartItemDTO> cartItems { get; set; }
        public string token { get; set; }
    }

    public class ListCartItemsWithTokenDTO
    {
        public List<CartItemDTO> cartItems { get; set; }
        public string token { get; set; }
    }
}