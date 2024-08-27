using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;

namespace EcommerceAPI.Filters
{
    public class ClientAuthorizeActionFilter : IAsyncActionFilter, IFilterMetadata
    {
        private readonly ClientsService _clientsService;

        public ClientAuthorizeActionFilter(ClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var reference = context.HttpContext.Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var cpf_cnpj = context.HttpContext.Request.Headers["cpf_cnpj"].ToString();
                if (string.IsNullOrEmpty(cpf_cnpj)) throw new Exception("Cpf_cnpj not found");
                if (!await _clientsService.CheckClient(cpf_cnpj)) throw new Exception("Client not found");
                await next();
            } else {
                await next();
            }
            
        }
    }
}