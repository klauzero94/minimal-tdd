using System.Net;

namespace Kernel.Exceptions;

[Serializable]
public class BusinessException : Exception
{
    public HttpStatusCode Status { get; private set; }
    public override string Message { get; }
    public string ErrorCode { get; private set; }
    public BusinessException(HttpStatusCode? status = null, string? message = null, string? errorCode = null)
    {
        Status = status ?? HttpStatusCode.InternalServerError;
        Message = message ?? "Ocorreu um erro interno no servidor.";
        ErrorCode = errorCode ?? "ER0001";
    }
}