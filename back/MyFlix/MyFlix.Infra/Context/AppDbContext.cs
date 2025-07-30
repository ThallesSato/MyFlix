using Microsoft.EntityFrameworkCore;
using MyFlix.Domain.Models;

namespace MyFlix.Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Filme> Filmes { get; set; }
}