using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class CreateOrderDTO
    {
        public IEnumerable<OrderItemDTO>? items { get; set; }
        public int client_id { get; set; }
        public int seller_id { get; set; }
        public string delivery_type { get; set; }
        public string order_status { get; set; } = "pending confirmation";
        public decimal total_price { get; set; } = 0;
    }

    public class CreatedOrderDTO
    {
        public IEnumerable<OrderItemDTO>? items { get; set; }
        public int client_id { get; set; }
        public int seller_id { get; set; }
        public string delivery_type { get; set; }
        public string order_status { get; set; } = "pending_confirmation";
        public decimal? total_price { get; set; }
        public int id { get; set; }
    }

    public class GetOrderDTO
    {
        public int id {get; set;}
        public int client_id {get; set;}
        public int seller_id {get; set;}
        public string delivery_type {get; set;}
        public string order_status {get; set;}
        public decimal total_price {get; set;}
        // [NotMapped]
        public string? client_name {get; set;}
        // [NotMapped]
        public string? seller_name {get; set;}
        public IEnumerable<OrderItemDTO>? order_items {get; set;}
    }
    
    public class GetOrderWithTokenDTO
    {
        public int id {get; set;}
        public int client_id {get; set;}
        public int seller_id {get; set;}
        public string delivery_type {get; set;}
        public string order_status {get; set;}
        public decimal total_price {get; set;}
        // [NotMapped]
        public string? client_name {get; set;}
        // [NotMapped]
        public string? seller_name {get; set;}
        public IEnumerable<OrderItemDTO>? order_items {get; set;}
        public string token {get; set;}
    }

    public class GetOrdersDetailedDTO 
    {
        public int id {get; set;}
        public int client_id {get; set;}
        public int seller_id {get; set;}
        public string delivery_type {get; set;}
        public string order_status {get; set;}
        public decimal total_price {get; set;}
        // [NotMapped]
        public string client_name {get; set;}
        // [NotMapped]
        public string seller_name {get; set;}
        public IEnumerable<OrderItemDTO>? order_items {get; set;}
    }

    public class GetOrdersDetailedWithTokenDTO
    {
        public List<GetOrdersDetailedDTO>? orders {get; set;}
        public string token {get; set;}
    }

    public class GetByClientIDDTO
    {
        public int client_id {get; set;}
    }

    public class GetByCpfCnpjDTO
    {
        public string cpf_cnpj {get; set;}
    }

    public class GetOrdersByCpfCnpjDTO
    {
        public int id {get; set;}
        public int client_id {get; set;}
        public int seller_id {get; set;}
        public string delivery_type {get; set;}
        public string order_status {get; set;}
        public decimal total_price {get; set;}
        // [NotMapped]
        public string client_name {get; set;}
        // [NotMapped]
        public string seller_name {get; set;}
    }

    public class UpdateOrderStatusDTO
    {
        public int id {get; set;}
        public string order_status {get; set;}
    }

    public class OrderIdDTO
    {
        public int id {get; set;}
    }
}