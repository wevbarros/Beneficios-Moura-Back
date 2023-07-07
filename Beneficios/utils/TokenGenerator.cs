using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Beneficios.Utils;
public static class TokenGenerator
{
  public static string GenerateToken(string id, string email, string matricula, string nome)
  {
    var chaveSecreta = ("sua-chave-secreta-com-pelo-menos-32-bytes");
    var emissor = "seu-emissor";
    var publicoAlvo = "seu-publico-alvo";
    var expiracao = DateTime.UtcNow.AddHours(1);
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
    var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
    var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        emissor,
        publicoAlvo,
        claims,
        expires: expiracao,
        signingCredentials: credenciais
    );
    var tokenJwt = new JwtSecurityTokenHandler().WriteToken(token);

    return tokenJwt;
  }
}
