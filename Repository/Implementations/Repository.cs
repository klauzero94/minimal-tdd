using System.Linq.Expressions;
using System.Net;
using Kernel.Attributes;
using Kernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interfaces;

namespace Repository.Implementations;

public abstract class Repository<TCollection> : IRepository<TCollection> where TCollection : class
{
    private readonly IContext _context;
    private readonly IMongoCollection<TCollection> _collection;
    public Repository(IContext context)
    {
        _context = context;
        _collection = _context.GetCollection<TCollection>(GetCollectionName(typeof(TCollection)));
    }

    private string GetCollectionName(Type type)
    {
        return ((BsonCollectionAttribute)type.GetCustomAttributes(typeof(BsonCollectionAttribute), true).First())?.CollectionName
            ?? throw new BusinessException(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar estabelecer uma conexão com o banco de dados.");
    }

    public virtual void InsertOne(TCollection collection) => _context.AddCommand(() => _collection.InsertOneAsync(collection));

    public async virtual Task<TOutput> GetByIdAsync<TOutput>(ObjectId id) where TOutput : class =>
        await _collection.Aggregate().Match(Builders<TCollection>.Filter.Eq("_id", id)).As<TOutput>().FirstOrDefaultAsync();

    public async virtual Task<bool> Any(Expression<Func<TCollection, bool>> filter) => await _collection.Find(filter).AnyAsync();

    public virtual void ReplaceOne(TCollection collection, ObjectId id)
    {
        var filter = Builders<TCollection>.Filter.Eq("_id", id);
        _context.AddCommand(() => _collection.ReplaceOneAsync(filter, collection));
    }

    public async virtual Task<(IEnumerable<TOutput>, int total)> FindAsync<TOutput>(FilterDefinition<TCollection> filter,
        int? page, int? size) where TOutput : class
    {
        page = page == null ? 1 : page;
        size = size == null ? 10 : size;
        var total = _collection.Aggregate().Match(filter).Count();
        var output = await _collection.Find(filter).Skip((page.Value - 1) * size.Value).Limit(size.Value).As<TOutput>().ToListAsync();
        return (output, (int)(!total.Any() ? 0 : total.First().Count));
    }

    public virtual void DeleteOne(ObjectId id)
    {
        var filter = Builders<TCollection>.Filter.Eq("_id", id);
        _context.AddCommand(() => _collection.DeleteOneAsync(filter));
    }

    public void Dispose() => _context?.Dispose();
}