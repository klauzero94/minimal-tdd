using App.Inputs;
using App.Outputs;
using Kernel;
using Kernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Routes;

public class ProductRoutes
{
    private IProductService _productService;
    public ProductRoutes(IProductService productService) => _productService = productService;

    public void MapActions(WebApplication app)
    {
        #region Create Product
        app.MapPost(Paths.CreateProduct.Path, [AllowAnonymous] async (
            [FromBody] ProductInput model) =>
            Results.Created(Paths.CreateProduct.Path, new Response(
                "201", true, data: await _productService.CreateProduct(model)
            )))
        .Produces<Response<object, object>>(StatusCodes.Status201Created)
        .Produces<Response<object, Error>>(StatusCodes.Status400BadRequest)
        .Produces<Response<object, Error>>(StatusCodes.Status409Conflict)
        .Produces<Response<object, Error>>(StatusCodes.Status429TooManyRequests)
        .Produces<Response<object, Error>>(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting(RateLimitingPolicy.Fixed)
        .WithName(Paths.CreateProduct.Name)
        .WithTags(Paths.CreateProduct.Tags);
        #endregion

        #region Update Product
        app.MapPut(Paths.UpdateProduct.Path, [AllowAnonymous] async (
            [FromRoute] string id,
            [FromBody] ProductInput model) =>
            Results.Created(Paths.UpdateProduct.Path, new Response(
                "201", true, data: await _productService.UpdateProduct(model, id)
            )))
        .Produces<Response<object, object>>(StatusCodes.Status201Created)
        .Produces<Response<object, Error>>(StatusCodes.Status400BadRequest)
        .Produces<Response<object, Error>>(StatusCodes.Status409Conflict)
        .Produces<Response<object, Error>>(StatusCodes.Status429TooManyRequests)
        .Produces<Response<object, Error>>(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting(RateLimitingPolicy.Fixed)
        .WithName(Paths.UpdateProduct.Name)
        .WithTags(Paths.UpdateProduct.Tags);
        #endregion

        #region Get Products
        app.MapGet(Paths.GetProducts.Path, [AllowAnonymous] async (
            [FromQuery] string? name,
            [FromQuery] int? page,
            [FromQuery] int? size,
            HttpContext httpContext) =>
        {
            var output = await _productService.GetProducts(name, page, size);
            httpContext.Response.Headers.Add(HeadersProp.XTotalCount, output.Item2.ToString());
            return Results.Ok(new Response(
                "200", true, data: output.Item1
            ));
        })
        .Produces<Response<List<ProductOutput>, object>>(StatusCodes.Status200OK)
        .Produces<Response<object, Error>>(StatusCodes.Status204NoContent)
        .Produces<Response<object, Error>>(StatusCodes.Status429TooManyRequests)
        .Produces<Response<object, Error>>(StatusCodes.Status500InternalServerError)
        .WithName(Paths.GetProducts.Name)
        .WithTags(Paths.GetProducts.Tags);
        #endregion

        #region Get Product
        app.MapGet(Paths.GetProduct.Path, [AllowAnonymous] async (
            [FromRoute] string id,
            HttpContext httpContext) =>
            Results.Ok(new Response(
                "201", true, data: await _productService.GetProduct(id)
            )))
        .Produces<Response<List<ProductOutput>, object>>(StatusCodes.Status200OK)
        .Produces<Response<object, Error>>(StatusCodes.Status204NoContent)
        .Produces<Response<object, Error>>(StatusCodes.Status429TooManyRequests)
        .Produces<Response<object, Error>>(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting(RateLimitingPolicy.Fixed)
        .WithName(Paths.GetProduct.Name)
        .WithTags(Paths.GetProduct.Tags);
        #endregion

        #region Delete Product
        app.MapDelete(Paths.DeleteProduct.Path, [AllowAnonymous] async (
            [FromRoute] string id) =>
            {
                await _productService.DeleteProduct(id);
                return Results.Ok(new Response("200", true));
            })
        .Produces<Response<object, object>>(StatusCodes.Status201Created)
        .Produces<Response<object, Error>>(StatusCodes.Status400BadRequest)
        .Produces<Response<object, Error>>(StatusCodes.Status409Conflict)
        .Produces<Response<object, Error>>(StatusCodes.Status429TooManyRequests)
        .Produces<Response<object, Error>>(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting(RateLimitingPolicy.Fixed)
        .WithName(Paths.DeleteProduct.Name)
        .WithTags(Paths.DeleteProduct.Tags);
        #endregion
    }
}

#region Paths
public static class Paths
{
    public static class CreateProduct
    {
        public const string Path = "products";
        public const string Name = "Create Product";
        public static readonly string[] Tags = { "Products" };
    }

    public static class UpdateProduct
    {
        public const string Path = "products/{id}";
        public const string Name = "Update Product";
        public static readonly string[] Tags = { "Products" };
    }

    public static class GetProducts
    {
        public const string Path = "products";
        public const string Name = "Get Products";
        public static readonly string[] Tags = { "Products" };
    }

    public static class GetProduct
    {
        public const string Path = "products/{id}";
        public const string Name = "Get Product";
        public static readonly string[] Tags = { "Products" };
    }

    public static class DeleteProduct
    {
        public const string Path = "products/{id}";
        public const string Name = "Delete Product";
        public static readonly string[] Tags = { "Products" };
    }
}
#endregion