using App.Inputs;
using AutoMapper;
using Domain.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace App.Mapper;

public static class Mapper
{
    public static void Add(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductInput, ProductCollection>();
        });
        IMapper mapper = mapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);
    }
}