using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class PayDTO
    {
        public int? id { get; set; }
        public string description { get; set; }
        public decimal? value { get; set; }
        public string? status { get; set; }
        public string card_id { get; set; }
        public string card_type { get; set; }
        public CardInfoDTO card_info { get; set; }
    }
}