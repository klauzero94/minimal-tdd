using System.Net;
using App.Inputs;
using App.Outputs;
using AutoMapper;
using Domain.Collections;
using Kernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUoW _uow;
    private readonly IMapper _mapper;
    public ProductService(IProductRepository productRepository, IUoW uow, IMapper mapper)
    {
        _productRepository = productRepository;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ProductOutput> CreateProduct(ProductInput input)
    {
        var checkIfProductExists = await _productRepository.Any(x => x.Name == input.Name);
        if (checkIfProductExists)
            throw new BusinessException(HttpStatusCode.Conflict, "Já existe um produto com esse nome.", "EPS0001");
        var collection = _mapper.Map<ProductCollection>(input);
        _productRepository.InsertOne(collection);
        await _uow.CommitAsync();
        return await _productRepository.GetByIdAsync<ProductOutput>(collection.Id);
    }

    public async Task<ProductOutput> UpdateProduct(ProductInput input, string id)
    {
        var checkIfProductExists = await _productRepository.Any(x => x.Name == input.Name && x.Id != ObjectId.Parse(id));
        if (checkIfProductExists)
            throw new BusinessException(HttpStatusCode.Conflict, "Já existe um produto com esse nome.", "EPS0002");
        var collection = _mapper.Map<ProductCollection>(input);
        collection.Id = ObjectId.Parse(id);
        collection.UpdatedAt = DateTime.UtcNow;
        _productRepository.ReplaceOne(collection, collection.Id);
        await _uow.CommitAsync();
        return await _productRepository.GetByIdAsync<ProductOutput>(collection.Id);
    }

    public async Task<(IEnumerable<ProductOutput>, int total)> GetProducts(string? name, int? page, int? size)
    {
        var filter = Builders<ProductCollection>.Filter.Empty;
        if (!string.IsNullOrEmpty(name))
            filter = filter & Builders<ProductCollection>.Filter.Regex(x => x.Name, new BsonRegularExpression(name, "i"));
        var output = await _productRepository.FindAsync<ProductOutput>(filter, page, size);
        if (output.Item2 < 1)
            throw new BusinessException(HttpStatusCode.NoContent, "Não foi encontrado nenhum dado com os parâmetros informados.", "EPS0003");
        return (output.Item1, output.Item2);
    }

    public async Task<ProductOutput> GetProduct(string id)
    {
        var output = await _productRepository.GetByIdAsync<ProductOutput>(ObjectId.Parse(id));
        if (output == null)
            throw new BusinessException(HttpStatusCode.NoContent, "Não foi encontrado nenhum dado com os parâmetros informados.", "EPS0004");
        return output;
    }

    public async Task DeleteProduct(string id)
    {
        var checkIfProductExists = await _productRepository.Any(x => x.Id == ObjectId.Parse(id));
        if (!checkIfProductExists)
            throw new BusinessException(HttpStatusCode.Conflict, "O produto não existe.", "EPS0005");
        _productRepository.DeleteOne(ObjectId.Parse(id));
        await _uow.CommitAsync();
    }
}