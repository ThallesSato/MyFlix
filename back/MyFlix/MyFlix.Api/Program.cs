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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
        return Results.BadRequest(e.Message);
    }
});

app.MapPost("/Filme",
    async ([FromBody] FilmeDto filmeDto, [FromServices] IValidator<FilmeDto> validator, IFilmeService filmeService, IUnitOfWork unitOfWork) =>
    {
        ValidationResult validationResult = await validator.ValidateAsync(filmeDto);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        try
        {
            await filmeService.PostFilmeAsync(filmeDto);
            await unitOfWork.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    });

app.MapPut("Filme/{id}", async ([FromRoute] int id,[FromBody] Filme filme, [FromServices] IValidator<Filme> validator, IFilmeService filmeService, IUnitOfWork unitOfWork) =>
{
    ValidationResult validationResult = await validator.ValidateAsync(filme);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    if (!await filmeService.UpdateFilmeByIdAsync(id, filme))
        return Results.NotFound();
        
    await unitOfWork.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("Filme/Nota/{id}", async ([FromRoute] int id,[FromBody] int nota, [FromServices] IFilmeService filmeService, IUnitOfWork unitOfWork) =>
{
    if (nota<1 || nota>5)
    {
        return Results.BadRequest("A nota deve estar entre 1 e 5.");
    }

    if (!await filmeService.AdicionaNotaAoFilmeByIdAsync(id, nota))
        return Results.NotFound();
        
    await unitOfWork.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("Filme/{id}",
    async ([FromRoute] int id, [FromServices] IFilmeService filmeService, IUnitOfWork unitOfWork) =>
    {

        try
        {
            var deleted = await filmeService.DeleteFilmeByIdAsync(id);
            if (!deleted) return Results.NotFound();
            await unitOfWork.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    });

app.Run();