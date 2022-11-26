using MongoDB.Driver;

namespace Repository.Interfaces;

public interface IContext : IDisposable
{
    void AddCommand(Func<Task> func);
    Task<int> SaveChangesAsync();
    IMongoCollection<T> GetCollection<T>(string name);
}