using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceAPI.Filters
{
    public class AuthorizeActionFilter : ActionFilterAttribute
    {
        private readonly TokenService _tokenService;
        public AuthorizeActionFilter(TokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task OnActionExecuting(ActionExecutingContext context)
        {
            System.Console.WriteLine("Executando Action Filter");
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ObjectResult(new{
                    status = 403,
                    message = "Autenticado mas nao autorizado"
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
            else{
                var newToken = await _tokenService.GenerateAndSaveToken();
            }
        }
    
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // if (context.Result is ObjectResult result)
            // {
            //     var token = await _tokenService.GenerateAndSaveToken();
            //     result.Value = new
            //     {
            //         result.Value,
            //         token
            //     };
            // }
            
            Console.WriteLine("LogActionFilter executada através do Action Filter");
        }
    }
}