using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UniverseRepo.Configurations;

public static class SwaggerServiceCollection
{
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Repository Universe",
                Version = "v1"
            });

            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira o token JWT precedido de 'Bearer'.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            x.OperationFilter<AuthResponsesOperationFilter>();

            x.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
            x.TagActionsBy(api => new[] { api.GroupName });

            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
    }

    private class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.DeclaringType != null &&
                !context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute) &&
                !context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    }
                };
            }
        }
    }
}