using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFlix.Application.Dtos.Input;
using MyFlix.Application.Interfaces;
using MyFlix.Application.Services;
using MyFlix.Application.Validators;
using MyFlix.Domain.Models;
using MyFlix.Infra.Context;
using MyFlix.Infra.Interfaces;
using MyFlix.Infra.Repositories;
using Microsoft.OpenApi.Models;
using MyFlix.Application.Dtos.Output;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "MyFlix API", 
        Version = "v1",
        Description = "API para gerenciamento de filmes do MyFlix"
    });
    
    c.MapType<ErroDto>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            { "mensagem", new OpenApiSchema { Type = "string" } },
            { "statusCode", new OpenApiSchema { Type = "integer", Format = "int32" } }
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source = Database"));
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IValidator<FilmeDto>, FilmeDtoValidator>();
builder.Services.AddScoped<IValidator<Filme>, FilmeValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapGet("/Filme", async ([FromServices] IFilmeService filmeService) =>
{
    try
    {
        return Results.Ok(await filmeService.GetAllFilmesAsync());
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.BadRequest(new ErroDto(e.Message, StatusCodes.Status400BadRequest));
    }

})
.WithName("GetFilmes")
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Retorna todos os filmes",
    Description = "Obtém uma lista de todos os filmes cadastrados no sistema"
})
.Produces<List<Filme>>()
.Produces<ErroDto>(400);

app.MapPost("/Filme",
    async ([FromBody] FilmeDto filmeDto, [FromServices] IValidator<FilmeDto> validator, IFilmeService filmeService, IUnitOfWork unitOfWork) =>
    {
        ValidationResult validationResult = await validator.ValidateAsync(filmeDto);

        if (!validationResult.IsValid)
        {
            var erros = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Results.BadRequest(new ErroDto(string.Join(", ", erros), StatusCodes.Status400BadRequest));
        }

        try
        {
            await filmeService.PostFilmeAsync(filmeDto);
            await unitOfWork.SaveChangesAsync();
            return Results.Created();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new ErroDto(e.Message, StatusCodes.Status400BadRequest));
        }
    })
.WithName("CreateFilme")
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Cria um novo filme",
    Description = "Adiciona um novo filme ao catálogo"
})
.Produces(201)
.Produces<ErroDto>(400);

app.MapPut("Filme/{id}", async ([FromRoute] int id, [FromBody] Filme filme, [FromServices] IValidator<Filme> validator, IFilmeService filmeService, IUnitOfWork unitOfWork) =>
{
    ValidationResult validationResult = await validator.ValidateAsync(filme);

    if (!validationResult.IsValid)
    {
        var erros = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return Results.BadRequest(new ErroDto(string.Join(", ", erros), StatusCodes.Status400BadRequest));
    }


    if (!await filmeService.UpdateFilmeByIdAsync(id, filme))
        return Results.NotFound(new ErroDto("Filme não encontrado", StatusCodes.Status404NotFound));
    
    await unitOfWork.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateFilme")
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Atualiza um filme",
    Description = "Atualiza os dados de um filme existente pelo ID"
})
.Produces(204)
.Produces<ErroDto>(400)
.Produces<ErroDto>(404);

app.MapPut("Filme/Nota/{id}", async ([FromRoute] int id, [FromBody] int nota, [FromServices] IFilmeService filmeService, IUnitOfWork unitOfWork) =>
{
    if (nota < 1 || nota > 5)
    {
        return Results.BadRequest(new ErroDto("A nota deve estar entre 1 e 5", StatusCodes.Status400BadRequest));
    }

    if (!await filmeService.AdicionaNotaAoFilmeByIdAsync(id, nota))
        return Results.NotFound(new ErroDto("Filme não encontrado", StatusCodes.Status404NotFound));
    
    await unitOfWork.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("AtualizarNota")
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Atualiza a nota de um filme",
    Description = "Adiciona ou atualiza a nota de um filme (entre 1 e 5)"
})
.Produces(204)
.Produces<ErroDto>(400)
.Produces<ErroDto>(404);

app.MapDelete("Filme/{id}",
    async ([FromRoute] int id, [FromServices] IFilmeService filmeService, IUnitOfWork unitOfWork) =>
    {
        try
        {
            var deleted = await filmeService.DeleteFilmeByIdAsync(id);
            if (!deleted)
                return Results.NotFound(new ErroDto("Filme não encontrado", StatusCodes.Status404NotFound));
            
            await unitOfWork.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new ErroDto(e.Message, StatusCodes.Status400BadRequest));
        }
    })
.WithName("DeleteFilme")
.WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Remove um filme",
    Description = "Exclui um filme do catálogo pelo ID"
})
.Produces(204)
.Produces<ErroDto>(400)
.Produces<ErroDto>(404);

app.Run();