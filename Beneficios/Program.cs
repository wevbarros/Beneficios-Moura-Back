using Microsoft.EntityFrameworkCore;
using Beneficios.Services;
using Beneficios.Utils;
// using Beneficios.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BancoDeDados>(
    options => options.UseSqlite("Data Source = bancoDeDados.db")
);

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
  builder.AllowAnyOrigin().AllowAnyHeader();
}));

var app = builder.Build();
app.UseCors();
// app.UseAuthorizationMiddleware();

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
        var token = TokenGenerator.GenerateToken("1", "email", matricula, "elliot");
        return Results.Ok(new { token });
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Erro ao gerar token: {ex.Message}");
        return Results.StatusCode(500);
      }
    }
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
