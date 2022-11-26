using App.Inputs;
using App.Outputs;

namespace Service.Interfaces;

public interface IProductService
{
    Task<ProductOutput> CreateProduct(ProductInput input);
    Task<ProductOutput> UpdateProduct(ProductInput input, string id);
    Task<(IEnumerable<ProductOutput>, int total)> GetProducts(string? name, int? page, int? size);
    Task<ProductOutput> GetProduct(string id);
    Task DeleteProduct(string id);
}