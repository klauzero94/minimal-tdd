using Microsoft.Extensions.DependencyInjection;
using Service.Implementations;
using Service.Interfaces;

namespace IoC;

public static class IoCService
{
    public static void Configure(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
    }
}