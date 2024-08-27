using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class Token
    {
        public int id { get; set; }
        public required string token { get; set; }
    }
}