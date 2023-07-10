using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Beneficios.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("AuthorizationMiddleware: Início AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\nAAAAAAAAAAAAA");
            // Verifique se o cabeçalho Authorization está presente na solicitação
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                // Obtenha o valor do cabeçalho Authorization
                var authorizationHeader = context.Request.Headers["Authorization"].ToString();

                // Faça o processamento necessário no token de autorização aqui
                // Exemplo: Valide o token, extraia informações do token, verifique permissões, etc.

                // Você pode armazenar as informações extraídas do token no contexto, se desejar
                context.Items["AuthToken"] = authorizationHeader;
            }

            // Chame o próximo middleware na cadeia de execução
            await _next(context);
        }
    }

    // Classe de extensão para registrar o middleware
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
