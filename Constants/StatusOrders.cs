using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Constants
{
    public class StatusOrders
    {
        public const string PendingConfirmation = "pending_confirmation";
        public const string Confirmed = "confirmed";
        public const string AwaitingDelivery = "awaiting_delivery";
        public const string AwaitingPickup = "awaiting_pickup";
        public const string Delivered = "delivered";
        public const string Canceled = "canceled";
    }
}