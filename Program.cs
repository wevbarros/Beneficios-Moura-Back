using Microsoft.EntityFrameworkCore;

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
