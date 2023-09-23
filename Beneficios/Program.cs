using Microsoft.EntityFrameworkCore;
using Beneficios.Services;
using Beneficios.Utils;
using Beneficios.Middleware;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuração do IConfiguration para acessar o appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();


// Obtém a string de conexão do IConfiguration
var connectionString = configuration.GetConnectionString("DataBase");

var options = new DbContextOptionsBuilder<BancoDeDados>()
    .UseSqlServer(connectionString)
    .Options;

builder.Services.AddDbContext<BancoDeDados>(options => options.UseSqlServer(connectionString));

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
  builder.AllowAnyOrigin().AllowAnyHeader();
}));

var app = builder.Build();
app.UseCors();
app.UseMiddleware<JwtMiddleware>();

app.MapGet("/", () => "Hala Madrid!");

app.MapPost("/login", async (BancoDeDados dbContext, HttpContext context) =>
{

  if (!context.Request.HasJsonContentType())
  {
    return Results.BadRequest();
  }

  var requestBody = await context.Request.ReadFromJsonAsync<LoginRequest>();

  if (requestBody == null)
  {
    return Results.BadRequest();
  }
  else
  {
    if (requestBody.Matricula == null || requestBody.Password == null)
    {
      return Results.BadRequest();
    }
    else
    {
      try
      {
        var matricula = requestBody.Matricula;
        var password = requestBody.Password;

        var authService = new AuthService();
        var response = await authService.AuthenticateAsync(matricula, password);
        Console.WriteLine("Resposta: " + response);

        if (response)
        {
          User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == matricula);
          var token = JWT.GenerateToken(user.Id, user.Email, user.Matricula, user.Nome, user.CodLevel);
          return Results.Ok(new { token });
        }
        else
        {
          return Results.Unauthorized();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Erro ao gerar token: {ex.Message}");
        return Results.StatusCode(500);
      }
    }
  }
});

app.MapPost("/refreshToken", (BancoDeDados dbContext, HttpContext context) =>
{
  if (!context.Request.Headers.ContainsKey("Authorization"))
  {
    return Results.BadRequest();
  }
  var authorizationHeader = context.Request.Headers["Authorization"].ToString();
  bool isValid = JWT.ValidateToken(authorizationHeader);
  if (!isValid)
  {
    return Results.Unauthorized();
  }
  else
  {
    var decodedToken = JWT.DecodeToken(authorizationHeader);
    var user = System.Text.Json.JsonSerializer.Deserialize<User>(decodedToken.Claims.First().Value);

    if (user == null)
    {
      return Results.BadRequest();
    }
    else
    {

      if (user.Id == null || user.Matricula == null || user.Email == null || user.Nome == null)
      {
        return Results.BadRequest();
      }
      else
      {
        var token = JWT.GenerateToken(user.Id, user.Email, user.Matricula, user.Nome, user.CodLevel);
        return Results.Ok(new { token });
      }
    }
  }
});

app.MapPost("/verifyToken", (HttpContext context) =>
{
  if (!context.Request.Headers.ContainsKey("Authorization"))
  {
    return Results.BadRequest();
  }
  var authorizationHeader = context.Request.Headers["Authorization"].ToString();
  bool isValid = JWT.ValidateToken(authorizationHeader);
  if (!isValid)
  {
    return Results.Unauthorized();
  }
  else
  {
    return Results.Ok();
  }
});

app.MapPost("/cadastrarBeneficio", async (BancoDeDados bd, HttpContext context) =>
{
  try
  {
    var form = await context.Request.ReadFormAsync();

    var nome = form["nome"];
    var categoria = form["categoria"];
    var imagem = form.Files["imagem"];
    var descricao = form["descricao"];

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
    var filePath = Path.Combine("Uploads", fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await imagem.CopyToAsync(stream);
    }

    try
    {
      BlobStorageUploader uploader = new BlobStorageUploader();
      uploader.UploadImageToBlob(filePath);
      var beneficio = new Beneficio
      {
        Id = 0,
        Categoria = int.Parse(categoria),
        Nome = nome,
        Descricao = descricao,
        urlImage = "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/" + filePath.Substring(filePath.IndexOf("\\") + 1)
      };

      bd.Add(beneficio);
      await bd.SaveChangesAsync();
      return Results.Ok(beneficio);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erro ao fazer upload do blob: {ex.Message}");
      return Results.StatusCode(500);
    }
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
    return Results.StatusCode(500);
  }
});



app.MapGet("/beneficios/", async (BancoDeDados bd) =>
    await bd.beneficios.ToListAsync());

app.MapGet("/beneficios/{id}", async (int id, BancoDeDados bd) =>

    await bd.beneficios.FindAsync(id)
    is Beneficio beneficio ? Results.Ok(beneficio) : Results.NotFound());


app.MapPost("/beneficios/", async (BancoDeDados bd, Beneficio beneficio) =>
{
  bd.Add(beneficio);
  await bd.SaveChangesAsync();
  return Results.Ok(beneficio);
});

app.MapPost("/cadastrarBeneficios/", async (BancoDeDados bd, Beneficio beneficio) =>
{
  bd.Add(beneficio);
  await bd.SaveChangesAsync();
  return Results.Ok(beneficio);
});

app.MapPut("/beneficios/{id}", async (int id, BancoDeDados bd, HttpContext context) =>
{
  try
  {
    var form = await context.Request.ReadFormAsync();

    var nome = form["nome"];
    var categoria = form["categoria"];
    var imagem = form.Files["imagem"];
    var descricao = form["descricao"];

    bool isNomeVazio = string.IsNullOrEmpty(nome);
    bool isCategoriaVazia = string.IsNullOrEmpty(categoria);
    bool isImagemVazia = imagem == null || imagem.Length == 0;
    bool isDescricaoVazia = string.IsNullOrEmpty(descricao);

    try
    {
      var beneficio = await bd.beneficios.FindAsync(id);
      if (!isImagemVazia)
      {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
        var filePath = Path.Combine("Uploads", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await imagem.CopyToAsync(stream);
        }
        BlobStorageUploader uploader = new BlobStorageUploader();
        uploader.UploadImageToBlob(filePath);
        beneficio.urlImage = "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/" + filePath.Substring(filePath.IndexOf("\\") + 1);
      }

      if (!isNomeVazio) beneficio.Nome = nome;
      if (!isCategoriaVazia) beneficio.Categoria = int.Parse(categoria);
      if (!isDescricaoVazia) beneficio.Descricao = descricao;

      await bd.SaveChangesAsync();
      return Results.Ok(beneficio);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erro ao fazer upload do blob: {ex.Message}");
      return Results.StatusCode(500);
    }
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
    return Results.StatusCode(500);
  }
});

app.MapPut("/beneficios/img/{id}", async (int id, BancoDeDados bd, HttpContext context) =>
{

});

app.MapPut("/beneficios/desc/{id}", async (int id, BancoDeDados bd, Beneficio inputBen) =>
{
  var beneficio = await bd.beneficios.FindAsync(id);
  if (beneficio == null) return Results.NotFound();

  beneficio.Descricao = inputBen.Descricao;

  await bd.SaveChangesAsync();
  return Results.NoContent();
});

app.MapPut("/beneficios/nome/{id}", async (int id, BancoDeDados bd, Beneficio inputBen) =>
{
  var beneficio = await bd.beneficios.FindAsync(id);
  if (beneficio == null) return Results.NotFound();

  beneficio.Nome = inputBen.Nome;

  await bd.SaveChangesAsync();
  return Results.NoContent();
});

app.MapPut("/beneficios/cat/{id}", async (int id, BancoDeDados bd, Beneficio inputBen) =>
{
  var beneficio = await bd.beneficios.FindAsync(id);
  if (beneficio == null) return Results.NotFound();

  beneficio.Categoria = inputBen.Categoria;

  await bd.SaveChangesAsync();
  return Results.NoContent();
});


app.MapDelete("/beneficios/{id}", async (int id, BancoDeDados bd) =>
{
  if (await bd.beneficios.FindAsync(id) is Beneficio beneficio)
  {
    bd.beneficios.Remove(beneficio);
    await bd.SaveChangesAsync();
    return Results.Ok(beneficio);
  }
  return Results.NotFound();
});

app.Run();
