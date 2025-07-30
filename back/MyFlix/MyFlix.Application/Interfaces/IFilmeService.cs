using MyFlix.Application.Dtos.Input;
using MyFlix.Domain.Models;

namespace MyFlix.Application.Interfaces;

public interface IFilmeService
{
    Task<List<Filme>> GetAllFilmesAsync();
    Task PostFilmeAsync(FilmeDto filmeDto);
    Task<bool> DeleteFilmeByIdAsync(int id);
    Task<bool> UpdateFilmeByIdAsync(int id, Filme filme);
    Task<bool> AdicionaNotaAoFilmeByIdAsync(int id, int nota);
}