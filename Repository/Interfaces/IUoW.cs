namespace Repository.Interfaces;

public interface IUoW : IDisposable
{
    Task<bool> CommitAsync();
}