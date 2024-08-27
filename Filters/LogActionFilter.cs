using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceAPI.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Requisição feita para a rota {context.HttpContext.Request.Path}.");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("LogActionFilter executada.");
        }
    }
}