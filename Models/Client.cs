using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string? complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public IEnumerable<Order>? orders { get; set; }
        public IEnumerable<ShoppingCart>? shopping_carts { get; set; }
    }
}