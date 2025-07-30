using MyFlix.Domain.Models;

namespace MyFlix.Infra.Interfaces;

public interface IFilmeRepository
{
    Task<List<Filme>> GetAllFilmesAsync();
    Task<Filme?> GetFilmeByIdAsyncOrNull(int id);
    Task PostFilmeAsync(Filme filme);
    Task<bool> DeleteFilmeByIdAsync(int id);
    void UpdateFilme(Filme filme);
}