using Microsoft.EntityFrameworkCore;
using MyFlix.Domain.Models;
using MyFlix.Infra.Context;
using MyFlix.Infra.Interfaces;

namespace MyFlix.Infra.Repositories;

public class FilmeRepository  : IFilmeRepository
{
    private readonly AppDbContext _context;
    public FilmeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Filme>> GetAllFilmesAsync()
    {
        return await _context.Filmes.ToListAsync();
    }
    
    public async Task<Filme?> GetFilmeByIdAsyncOrNull(int id)
    {
        return await _context.Filmes.FindAsync(id);
    }
    
    public async Task PostFilmeAsync(Filme filme)
    {
        await _context.Filmes.AddAsync(filme);
    } 
    
    public async Task<bool> DeleteFilmeByIdAsync(int id)
    {
        var filme = await _context.Filmes.FindAsync(id);
        if (filme == null) return false;
        _context.Filmes.Remove(filme);
        return true;
    }
    
    public void UpdateFilme(Filme filme)
    {
         _context.Filmes.Update(filme);
    }
}