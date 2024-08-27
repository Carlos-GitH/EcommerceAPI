using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceAPI.Filters
{
    public class SellerAuthorizeActionFilter : IAsyncActionFilter, IFilterMetadata
    {
        private readonly SellersService _sellersService;

        public SellerAuthorizeActionFilter(SellersService sellersService)
        {
            _sellersService = sellersService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path;
            if (ClientRoutes.IsClientRoute(path))
            {
                await next();
            } else {
                var reference = context.HttpContext.Request.Headers["reference"].ToString();
                if (string.IsNullOrEmpty(reference)) throw new Exception("Reference not found");
                if (!await _sellersService.CheckSeller(reference)) throw new Exception("Seller not found");
            }
            
        }
    }
}