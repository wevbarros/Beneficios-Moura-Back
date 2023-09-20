using Beneficios.Utils;

namespace Beneficios.Middleware;
public class JwtMiddleware
{
  private readonly RequestDelegate _next;

  public JwtMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
{
    var path = context.Request.Path;
    var method = context.Request.Method;

    if (path.StartsWithSegments("/login") || path.StartsWithSegments("/") || path.StartsWithSegments("/beneficios") && method == HttpMethods.Get)
    {
        await _next(context); // Skip token validation for /login and /refreshToken endpoints
        return;
    }

    if (method != HttpMethods.Get && !context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }

    var authorizationHeader = context.Request.Headers["Authorization"].ToString();
    var token = authorizationHeader.Replace("Bearer ", string.Empty);

    if (string.IsNullOrWhiteSpace(token))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
    }
    else
    {
        // Valide o token JWT
        bool isValid = JWT.ValidateToken(token);

        if (!isValid)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
        else
        {
            context.Items["JwtToken"] = token;
            await _next(context);
        }
    }
}

}
