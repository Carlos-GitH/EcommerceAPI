using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class Seller
    {
        public int id { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public IEnumerable<Order>? orders { get; set; }
    }
}