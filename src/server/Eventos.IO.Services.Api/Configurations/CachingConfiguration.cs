using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class CachingConfiguration
    {
        public static void AddCachingConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            // Para saber mais sobre cache em memória acesse a documentação disponível no link abaixo
            // https://docs.microsoft.com/pt-br/aspnet/core/performance/caching/memory?view=aspnetcore-3.1
            services.AddMemoryCache();


            //var redis = Configuration.GetConnectionString("ConexaoRedis");

            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = redis.Endpoint;
            //    options.InstanceName = redis.InstanceName;
            //});
        }
    }
}
