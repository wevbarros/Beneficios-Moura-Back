using Microsoft.EntityFrameworkCore;
using Beneficios.Services;

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

app.MapGet("/", () => "Hala Madrid!");

app.MapPost("/cadastrarBeneficio", async (BancoDeDados bd, HttpContext context) =>
{
    try
    {
        var form = await context.Request.ReadFormAsync();

        var nome = form["nome"];
        var categoria = form["categoria"];
        var imagem = form.Files["imagem"];
        var descricao = form["descricao"];

        var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
        var caminhoArquivo = Path.Combine("Uploads", nomeArquivo);

        using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
        {
            await imagem.CopyToAsync(stream);
        }

        // try
        // {
        //     BlobStorageUploader uploader = new BlobStorageUploader();
        //     uploader.UploadImageToBlob(caminhoArquivo, nomeArquivo);
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Erro ao fazer upload do blob: {ex.Message}");
        //     // Lidar com o erro de upload do blob (como registrar, notificar, etc.)
        // }

        var beneficio = new Beneficio
        {
            Id = 0,
            Categoria = int.Parse(categoria),
            Nome = nome,
            Descricao = descricao,
            urlImage = caminhoArquivo
        };

        bd.Add(beneficio);
        await bd.SaveChangesAsync();

        return Results.Ok(beneficio);
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

app.MapPut("/beneficios/{id}", async (int id, BancoDeDados bd, Beneficio inputBen) =>
{
    var beneficio = await bd.beneficios.FindAsync(id);
    if (beneficio == null) return Results.NotFound();

    beneficio.Nome = inputBen.Nome;
    beneficio.Categoria = inputBen.Categoria;
    beneficio.Descricao = inputBen.Descricao;
    beneficio.urlImage = inputBen.urlImage;

    await bd.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/beneficios/img/{id}", async (int id, BancoDeDados bd, Beneficio inputBen) =>
{
    var beneficio = await bd.beneficios.FindAsync(id);
    if (beneficio == null) return Results.NotFound();

    beneficio.urlImage = inputBen.urlImage;

    await bd.SaveChangesAsync();
    return Results.NoContent();
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
