using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Constants;

namespace EcommerceAPI.Models
{
    public class ClientRoutes
    {
        public string clientRoutes { get; set; }

        public static bool IsClientRoute(string route)
        {
            return route.Contains(ClientRoutesConst.client)                 ||
                   route.Contains(ClientRoutesConst.personal_info)          ||
                   route.Contains(ClientRoutesConst.address)                ||
                   route.Contains(ClientRoutesConst.GetByCpfCnpj)           ||
                   route.Contains(ClientRoutesConst.getByEmail)             ||
                   route.Contains(ClientRoutesConst.editClient)             ||
                   route.Contains(ClientRoutesConst.DeleteClient)           ||
                   route.Contains(ClientRoutesConst.getOrderById)           ||
                   route.Contains(ClientRoutesConst.getOrdersByClientId)    ||
                   route.Contains(ClientRoutesConst.createOrder)            ||
                   route.Contains(ClientRoutesConst.createShoppingCart)     ||
                   route.Contains(ClientRoutesConst.addToShoppingCart)      ||
                   route.Contains(ClientRoutesConst.removeFromShoppingCart) ||
                   route.Contains(ClientRoutesConst.resetShoppingCart)      ||
                   route.Contains(ClientRoutesConst.buyShoppingCart)
                   ;
        }
        
    }
}