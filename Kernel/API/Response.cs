using System.Text.Json;

namespace Kernel.API;

public class Response<T, E>
{
    public Response(string statusCode, bool success, T? data, E? error)
    {
        StatusCode = statusCode;
        Success = success;
        Data = data;
        Error = error;
    }
    public string StatusCode { get; set; } = string.Empty;
    public bool Success { get; set; }
    public T? Data { get; set; }
    public E? Error { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}

public class Response
{
    public Response(string statusCode, bool success, object? data = null, object? error = null)
    {
        StatusCode = statusCode;
        Success = success;
        Data = data;
        Error = error;
    }
    public string StatusCode { get; set; } = string.Empty;
    public bool Success { get; set; }
    public object? Data { get; set; }
    public object? Error { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}