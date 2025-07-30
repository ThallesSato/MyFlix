using MyFlix.Domain.Models;

namespace MyFlix.Infra.Interfaces;

public interface IFilmeRepository
{
    Task<List<Filme>> GetAllFilmesAsync();
    Task PostFilmeAsync(Filme filme);
    Task<bool> DeleteFilmeByIdAsync(int id);
}