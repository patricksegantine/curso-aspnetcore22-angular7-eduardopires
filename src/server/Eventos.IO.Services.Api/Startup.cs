using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using Eventos.IO.Services.Api.Configurations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Eventos.IO.Services.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseConfig(Configuration);

            // Configurações de Autenticação, Autorização e JWT
            services.AddSecurityConfig(Configuration);

            // Caching InMemory e Redis
            services.AddCachingConfig(Configuration);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });

            // filtro de ações
            services.AddControllers(options =>
            {
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalActionLoggerFilter)));
            }).AddJsonOptions(options =>
            {
                // remove valores nulos e somente leitura do retorno da API
                var serializerOptions = options.JsonSerializerOptions;
                serializerOptions.IgnoreNullValues = true;
                serializerOptions.IgnoreReadOnlyProperties = true;
                serializerOptions.WriteIndented = true;
            });


            // Customizando o comportamento do ApiControllerAttribute
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressConsumesConstraintForFormFileParameters = true;
            });

            // Mapeamentos DE/PARA
            services.AddAutoMapperConfig();

            // MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            // Versionamento do WebApi
            services.AddApiVersioningConfig();

            // Swagger
            services.AddSwaggerConfig();

            // // ASP.NET HttpContext 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Registrar todos os DI
            services.AddDependencyInjectionConfig();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Este middleware analisa o conjunto de pontos finais definidos no aplicativo e 
            // seleciona a melhor correspondência com base na solicitação.
            app.UseRouting();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            // Ativa a compressão
            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseApiVersioning();

            app.UseSwaggerConfig(provider);
        }
    }
}
