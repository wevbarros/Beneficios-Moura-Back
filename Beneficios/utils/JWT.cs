using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Beneficios.Utils;
public static class JWT
{
  public static string GenerateToken(string id, string email, string matricula, string nome)
  {
    var secretWord = ("sua-chave-secreta-com-pelo-menos-32-bytes");
    var issuer = "Moura pra você";
    var expires = DateTime.UtcNow.AddHours(1);
    var user = new
    {
      id = id,
      email = email,
      matricula = matricula,
      nome = nome
    };

    var claims = new[]
    {
      new Claim("user", System.Text.Json.JsonSerializer.Serialize(user))
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretWord));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: null,
        claims: claims,
        expires: expires,
        signingCredentials: credentials
    );
    var tokenJwt = new JwtSecurityTokenHandler().WriteToken(token);

    return tokenJwt;
  }
  public static bool ValidateToken(string token)
  {
    var secretWord = "sua-chave-secreta-com-pelo-menos-32-bytes";

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretWord));

    var tokenHandler = new JwtSecurityTokenHandler();
    try
    {
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = true,
        ValidIssuer = "Moura pra você",
        ValidateAudience = false,
        // Outros parâmetros de validação, se necessário
      }, out SecurityToken validatedToken);
      return true;
    }
    catch
    {
      return false;
    }
  }

  public static ClaimsPrincipal DecodeToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var decodedToken = tokenHandler.ReadJwtToken(token);
    var claims = decodedToken.Claims;
    var identity = new ClaimsIdentity(claims);
    return new ClaimsPrincipal(identity);
  }
}
