﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                // Configura detalhes como a versão da API
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Eventos.IO API",
                    Version = "v1",
                    Description = "Exemplo de API REST criada com o ASP.NET Core",
                    Contact = new OpenApiContact 
                    { 
                        Name = "Patrick Segantine", 
                        Email = "", 
                        Url = new Uri("http://projetox.io/licensa") 
                    },
                    License = new OpenApiLicense 
                    { 
                        Name = "MIT", 
                        Url = new Uri("http://projetox.io/licensa") 
                    }
                });
            });
        }

        public static void UseSwaggerConfig(this IApplicationBuilder app)
        {
            // Ativando middlewares para uso do Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
