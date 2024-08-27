using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Constants;

namespace EcommerceAPI.Models
{
    public class Status
    {
        public string status { get; set; }

        public static bool IsValidStatus(string status)
        {
            return status == StatusOrders.PendingConfirmation ||
                   status == StatusOrders.Confirmed ||
                   status == StatusOrders.AwaitingDelivery ||
                   status == StatusOrders.AwaitingPickup ||
                   status == StatusOrders.Delivered ||
                   status == StatusOrders.Canceled;
        }

    }
}