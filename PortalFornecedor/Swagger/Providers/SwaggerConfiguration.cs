using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Swagger.Providers
{
    public static class SwaggerConfiguration
    {
        ///<summary>
        /// Configuração para swagger.
        /// </summary>
        /// 
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                     new OpenApiInfo
                     {
                         Title = "back-end Prefeituras",
                         Version = "v1",
                         Description = "API REST desenvolvida com ASP .NET Core 3.1 para a aplicação de busca de prefeituras",
                         Contact = new OpenApiContact
                         {
                             Name = "GSW - Integrated Solutions",
                             Url = new Uri("https://www.gsw.com.br"),
                             Email = "contato@softplus.com.br",
                         }
                     });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } },
                    { "ApiKey", new string[] { } },
                };

                options.AddSecurityDefinition(
                    "ApiKey",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Query,
                        Name = "api-key",
                        Description = "Utilização: Cole sua '{apiKey}'",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "ApiKey"
                    });

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Utilização: Escreva 'Bearer {seuToken}'",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                // Set the comments path for the Swagger JSON and UI.
                var _xmlPath = Path.Combine("wwwroot", "api-docs.xml");

                options.IncludeXmlComments(_xmlPath);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            return app.UseSwagger()
                        .UseSwaggerUI(c =>
                        {
                            /* Rota para acessar a documentação */
                            c.RoutePrefix = "documentation";

                            /* Não alterar: Configuração aplicável tanto para servidor quanto para localhost */
                            c.SwaggerEndpoint("../swagger/v1/swagger.json", "Documentação API v1");
                        });
        }
    }
}