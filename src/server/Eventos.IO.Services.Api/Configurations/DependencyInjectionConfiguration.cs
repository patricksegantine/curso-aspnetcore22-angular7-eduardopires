using Eventos.IO.Infra.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfig(this IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
