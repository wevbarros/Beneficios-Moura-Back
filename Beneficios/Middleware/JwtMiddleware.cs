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

    if (path.StartsWithSegments("/login"))
    {
      await _next(context); // Skip token validation for /login and /refreshToken endpoints
      return;
    }

    if (context.Request.Headers.ContainsKey("Authorization"))
    {
      var authorizationHeader = context.Request.Headers["Authorization"].ToString();
      var token = authorizationHeader.Replace("Bearer ", string.Empty);

      if (string.IsNullOrWhiteSpace(token))
      {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
      }
      else
      {
        // Validate the token
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
    else
    {
      context.Response.StatusCode = 401;
      await context.Response.WriteAsync("Unauthorized");
    }
  }
}
