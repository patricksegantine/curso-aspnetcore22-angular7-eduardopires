using Eventos.IO.Infra.CrossCutting.Identity.Data;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class DatabaseConfiguration
    {
        public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Contexto de Identidade
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Contexto principal
            services.AddDbContext<EventosContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Contexto de eventStore
            services.AddDbContext<EventStoreSqlContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
