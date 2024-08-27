using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class GetProductDTO()
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string? image_url { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
    }

    public class GetProductWithTokenDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string? image_url { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public string token { get; set; }
    }
    public class GetProductsDTO
    {
        public List<GetProductDTO> products { get; set; }
        public string token { get; set; }
    }
    public class CreateProductDTO()
    {
        public string title { get; set; }
        public string description { get; set; }
        public string? image_url { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
    }
    public class UpdateProductDTO()
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? image_url { get; set; }
        public decimal? price { get; set; }
        public int? stock { get; set; }
    }
}