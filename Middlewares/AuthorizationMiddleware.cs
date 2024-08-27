using EcommerceAPI.Models;
using EcommerceAPI.Services;

namespace EcommerceAPI.Middlewares
{

    public class AuthorizationMiddleware
    {
        // 1 - Injetar o método next: serve para dar prosseguimento à requisição
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // 2 - Criar um método Invoke ou InvokeAsync
        public async Task InvokeAsync(HttpContext context, TokenService tokenService)
        {
            string? token  = context.Request.Headers.Authorization;
            string? path   = context.Request.Path;
            string? method = context.Request.Method;

            Console.WriteLine($"Caminho: {path}");
            Console.WriteLine($"Método: {method}");
            Console.WriteLine($"Token: {token}");

            // Verifica se a rota é de login
            if (path != "/api/v1/clients/login" || path != "/api/v1/sellers/login")
            {
                if (!PublicRoutes.IsPublicRoute(path))
                {
                    // Verifica se o token foi passado no cabeçalho
                    if (token == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Acesso não autorizado. Erro: Faltando parâmetro token.");
                        return;
                    }

                    bool savedToken = await tokenService.VerifyToken(token);
                    if (!savedToken)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Acesso não autorizado. Erro: Token inválido ou inexistente");
                        return;
                    } else
                    {
                        await tokenService.DeleteToken(token);
                    }
                }
            }

            await _next(context);

        }
    }
    // public class LoggingMiddleware
    // {
    //     private readonly RequestDelegate _next;

    //     public LoggingMiddleware(RequestDelegate next)
    //     {
    //         _next = next;
    //     }

    //     public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    //     {
    //         string? token  = context.Request.Headers.Authorization;
    //         string? path   = context.Request.Path;
    //         string? method = context.Request.Method;

    //         System.Console.WriteLine($"Path: {context.Request.Path}");
    //         System.Console.WriteLine($"Method: {context.Request.Method}");
    //         System.Console.WriteLine("Token: " + token);

    //         if(path != "/api/v1/clients/login" || path != "/api/v1/sellers/login")
    //         {
    //             if(PublicRoutes.IsPublicRoute(path))
    //             {
    //                 System.Console.WriteLine("Rota liberada");
    //             } else if (!PublicRoutes.IsPublicRoute(path))
    //             {
    //                 var savedToken = dbContext.Tokens.FirstOrDefault(t => t.token == token);
    //                 if(savedToken == null)
    //                 {
    //                     context.Response.StatusCode = StatusCodes.Status403Forbidden;
    //                     await context.Response.WriteAsync("Acesso nao autorizado");
    //                     return;
    //                 } else {
    //                     dbContext.Tokens.Remove(savedToken);
    //                     await dbContext.SaveChangesAsync();
    //                 }
    //             } 
    //         }
            
    //         await _next(context);
    //     }
    // }
}