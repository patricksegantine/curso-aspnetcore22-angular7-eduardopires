using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                // Configura detalhes como a versão da API
                s.SwaggerDoc("v1", new Info
                {
                    Title = "Eventos.IO API",
                    Version = "v1",
                    Description = "Exemplo de API REST criada com o ASP.NET Core",
                    TermsOfService = "",
                    Contact = new Contact { Name = "Patrick Segantine", Email = "", Url = "http://projetox.io/licensa" },
                    License = new License { Name = "MIT", Url = "http://projetox.io/licensa" }
                });

                //s.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }
    }
}
