using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Constants;

namespace EcommerceAPI.Models
{
    public class PublicRoutes
    {
        public string publicRoutes { get; set; }

        public static bool IsPublicRoute(string route)
        {
            return route == PublicRoutesConst.product        || 
                   route == PublicRoutesConst.products       ||
                   route == PublicRoutesConst.teste          ||
                   route == PublicRoutesConst.registerClient ||
                   route == PublicRoutesConst.swagger        ||
                   route == PublicRoutesConst.swaggerData    ||
                   route == PublicRoutesConst.login;
        }
    }
}