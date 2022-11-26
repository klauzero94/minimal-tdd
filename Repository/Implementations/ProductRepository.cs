using Domain.Collections;
using Repository.Interfaces;

namespace Repository.Implementations;

public class ProductRepository : Repository<ProductCollection>, IProductRepository
{
    public ProductRepository(IContext context) : base(context)
    {

    }
}