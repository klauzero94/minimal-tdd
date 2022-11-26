using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Repository.Interfaces;

public interface IRepository<TCollection> : IDisposable where TCollection : class
{
    void InsertOne(TCollection collection);
    Task<TOutput> GetByIdAsync<TOutput>(ObjectId id) where TOutput : class;
    Task<bool> Any(Expression<Func<TCollection, bool>> filter);
    Task<(IEnumerable<TOutput>, int total)> FindAsync<TOutput>(FilterDefinition<TCollection> filter, int? page, int? size) where TOutput : class;
    void ReplaceOne(TCollection collection, ObjectId id);
    void DeleteOne(ObjectId id);
}