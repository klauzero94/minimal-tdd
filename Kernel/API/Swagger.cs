using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kernel.API;

public static class Swagger
{
    public static void AddSwagger(this IServiceCollection services, string title, string version)
    {
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer"
            });
            x.OperationFilter<AuthorizationOperationFilter>();
            x.EnableAnnotations();
            x.SchemaFilter<EnumSchemaFilter>();
        });
    }

    public static void SetSwagger(this IApplicationBuilder app, string envName, string title)
    {
        app.UseSwagger();
        if (envName != EnvName.Production)
        {
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", title);
            });
            app.UseReDoc(x =>
            {
                x.DocumentTitle = "/redoc";
                x.SpecUrl = "/swagger/v1/swagger.json";
            });
        }
    }

    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var isAuthorized = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
            var allowAnonymous = actionMetadata.Any(metadataItem => metadataItem is AllowAnonymousAttribute);
            if (!isAuthorized || allowAnonymous)
            {
                return;
            }

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
    }

    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name => model.Enum.Add(new OpenApiString($"{Convert.ToInt64(Enum.Parse(context.Type, name))} = {name}")));
            }
        }
    }
}