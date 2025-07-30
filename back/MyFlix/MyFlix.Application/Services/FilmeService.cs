using System.Diagnostics.CodeAnalysis;
using MyFlix.Application.Dtos.Input;
using MyFlix.Application.Interfaces;
using MyFlix.Domain.Models;
using MyFlix.Infra.Interfaces;

namespace MyFlix.Application.Services;

public class FilmeService : IFilmeService
{
    private readonly IFilmeRepository _filmeRepository;
    public FilmeService(IFilmeRepository filmeRepository)
    {
        _filmeRepository = filmeRepository;
    }
    public async Task<List<Filme>> GetAllFilmesAsync()
    {
        return await _filmeRepository.GetAllFilmesAsync();
    }
    public async Task PostFilmeAsync(FilmeDto filmeDto)
    {
        var filme = new Filme
        {
            Titulo = filmeDto.Titulo,
            AnoLancamento = filmeDto.AnoLancamento,
            Genero = filmeDto.Genero
        };
        await _filmeRepository.PostFilmeAsync(filme);
    }
    
    public async Task<bool> DeleteFilmeByIdAsync(int id)
    {
        return await _filmeRepository.DeleteFilmeByIdAsync(id);
    }
    
    public async Task<bool> UpdateFilmeByIdAsync(int id, Filme filme)
    {
        var filmeOld = await _filmeRepository.GetFilmeByIdAsyncOrNull(id);
        if (filmeOld == null) 
            return false;
        
        filmeOld.Titulo = filme.Titulo;
        filmeOld.AnoLancamento = filme.AnoLancamento;
        filmeOld.Genero = filme.Genero;
        filmeOld.Status = filme.Status;
        filmeOld.Nota = filme.Nota;
        
        _filmeRepository.UpdateFilme(filmeOld);
        return true;
    }
}