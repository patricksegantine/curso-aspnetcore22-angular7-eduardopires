using Eventos.IO.Infra.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
