using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class SellerDTO
    {
        public class CreateSellerDTO
        {
            public string name { get; set; }
            public string reference { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }

        public class GetSellersDTO
        {
            public List<GetSellerDTO> sellers { get; set; }
            public string token { get; set; }
        }

        public class GetSellerDTO
        {
            public int id { get; set; }
            public string name { get; set; }
            public string reference { get; set; }
            public string? phone { get; set; }
            public string email { get; set; }
        }

        public class GetSellerWithTokenDTO
        {
            public int id { get; set; }
            public string name { get; set; }
            public string reference { get; set; }
            public string? phone { get; set; }
            public string email { get; set; }
            public string token { get; set; }
        }

        public class GetSellerByEmailDTO
        {
            public string email { get; set; }
        }

        public class GetSellerByReferenceDTO
        {
            public string reference { get; set; }
        }

        public class UpdateSellerDTO
        {
            public string? name { get; set; }
            public string? phone { get; set; }
            public string? email { get; set; }
        }

        public class LoginSellerDTO
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class LogguedSellerDTO
        {
            public string token { get; set; }
            public int id { get; set; }
            public string email { get; set; }
        }
    }
}