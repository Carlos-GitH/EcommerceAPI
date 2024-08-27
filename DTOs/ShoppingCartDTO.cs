using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class ShoppingCartDTO
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public decimal total_price {get; set; }
        public ICollection<CartItemDTO>? cart_items { get; set; }
    }

    public class ShoppingCartWithTokenDTO
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public decimal total_price {get; set; }
        public ICollection<CartItemDTO>? cart_items { get; set; }
        public string token { get; set; }
    }

    public class ShoppingCartsDTO
    {
        public ICollection<ShoppingCartDTO>? shopping_carts { get; set; }
        public string token { get; set; }
    }

    public class CreateShoppingCartDTO
    {
        public int client_id { get; set; }
    }
    
    public class CreatedShoppingCartDTO
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public string token { get; set; }
    }

    public class RemoveCartItemFromShoppingCartDTO
    {
        public int id { get; set; }
    }

}