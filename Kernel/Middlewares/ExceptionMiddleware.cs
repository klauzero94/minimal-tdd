using System.Net;
using Kernel.API;
using Kernel.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Kernel.Middlewares;

public static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this WebApplication app, string envName)
    {
        app.UseExceptionHandler(e =>
        {
            e.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (envName == EnvName.Production || envName == EnvName.Staging)
                    {
                        var statusCode = ((BusinessException)contextFeature.Error).Status;
                        await context.Response.WriteAsync(new Response(((int)statusCode).ToString(), false, error: new Error
                        {
                            Message = ((BusinessException)contextFeature.Error).Message,
                            Code = ((BusinessException)contextFeature.Error).ErrorCode
                        }).ToString());
                    }
                    else
                    {
                        var statusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new Response(statusCode.ToString(), false, error: new Error
                        {
                            Message = contextFeature.Error.StackTrace ?? contextFeature.Error.Message,
                            Code = contextFeature.Error.HResult.ToString()
                        }).ToString());
                    }
                }
            });
        });
    }
}