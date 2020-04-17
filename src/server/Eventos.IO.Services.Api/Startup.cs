using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using Eventos.IO.Services.Api.Configurations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Eventos.IO.Services.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            services.AddApiVersioning("api/v{version}");
            
            // Swagger
            services.AddSwaggerConfig();

            // // ASP.NET HttpContext 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Registrar todos os DI
            services.AddDependencyInjectionConfig();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

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

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseSwaggerConfig();
        }
    }
}
