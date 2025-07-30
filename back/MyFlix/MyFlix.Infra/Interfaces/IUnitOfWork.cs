namespace MyFlix.Infra.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}