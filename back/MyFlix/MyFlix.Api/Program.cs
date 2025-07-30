using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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