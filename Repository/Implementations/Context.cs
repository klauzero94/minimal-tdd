using System.Net;
using Kernel.Exceptions;
using Kernel.Settings;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Repository.Interfaces;

namespace Repository.Implementations;

public class Context : IContext
{
    private IMongoDatabase? _database { get; set; }
    public IClientSessionHandle? _session { get; set; }
    public MongoClient? _mongoClient { get; set; }
    private readonly List<Func<Task>> _commands;
    private readonly IConfiguration _configuration;

    public Context(IConfiguration configuration)
    {
        _configuration = configuration;
        _commands = new();
    }

    public async Task<int> SaveChangesAsync()
    {
        ConfigureMongo();
        if (_mongoClient is null) throw new BusinessException(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar configurar a conexão com o banco de dados.");
        using (_session = await _mongoClient.StartSessionAsync())
        {
            try
            {
                _session.StartTransaction();
                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);
                await _session.CommitTransactionAsync();

            }
            catch
            {
                await _session.AbortTransactionAsync();
                throw new BusinessException(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar salvar as alterações.");
            }
            finally
            {
                _commands.Clear();
                _session.Dispose();
            }
        }
        return _commands.Count;
    }

    private void ConfigureMongo()
    {
        if (_mongoClient != null) return;
        _mongoClient = new MongoClient(_configuration.GetConnectionString("Data"));
        _database = _mongoClient.GetDatabase(_configuration.GetSection("MongoDb").Get<MongoDbSettings>()?.Name);
    }

    public IMongoCollection<TCollection> GetCollection<TCollection>(string name)
    {
        ConfigureMongo();
        if (_database is null) throw new Exception("O banco de dados não existe.");
        return _database.GetCollection<TCollection>(name);
    }

    public void Dispose()
    {
        _session?.Dispose();
        GC.SuppressFinalize(this);
    }

    public void AddCommand(Func<Task> func) => _commands.Add(func);
}