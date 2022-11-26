using Microsoft.Extensions.DependencyInjection;
using Repository.Implementations;
using Repository.Interfaces;

namespace IoC;

public static class IoCRepository
{
    public static void Configure(this IServiceCollection services)
    {
        services.AddScoped<IContext, Context>();
        services.AddScoped<IUoW, UoW>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}