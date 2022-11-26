using System.Net;
using System.Text.Json;
using Kernel.API;
using Microsoft.AspNetCore.Http;

namespace Kernel.Middlewares;

public class ResponseMiddleware
{
    // todo: pode ser otimizado para ambientes
    private readonly RequestDelegate Next;
    public ResponseMiddleware(RequestDelegate next) => Next = next;
    public async Task Invoke(HttpContext context)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        await Next(context);
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            var response = new Response("401", false, error: new Error
            {
                Message = "Você não tem permissão pra acessar esse recurso. :(",
                Code = "ERR0001"
            });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
        if (context.Response.StatusCode == (int)HttpStatusCode.TooManyRequests)
        {
            var response = new Response("429", false, error: new Error
            {
                Message = "Limite de requisições atingido, aguarde um momento e tente novamente.",
                Code = "ERR0009"
            });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}