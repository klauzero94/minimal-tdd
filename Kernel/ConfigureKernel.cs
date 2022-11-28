using System.Threading.RateLimiting;
using Kernel.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace Kernel;

public static class ConfigureKernel
{
    public static void Configure(this IServiceCollection services)
    {
        services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: RateLimitingPolicy.Fixed, options =>
            {
                options.PermitLimit = 20;
                options.Window = TimeSpan.FromSeconds(5);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 0;
            }).RejectionStatusCode = 429);
    }

    public static void UseMiddlewares(this WebApplication app, string envName)
    {
        app.UseMiddleware<ResponseMiddleware>();
        ExceptionMiddleware.ConfigureExceptionHandler(app, envName);
        app.UseRateLimiter();
    }
}