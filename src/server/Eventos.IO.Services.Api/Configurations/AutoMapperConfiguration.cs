using AutoMapper;
using Eventos.IO.Services.Api.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //var mappingConfig = AutoMapperConfig.RegisterMappings();
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
        }
    }
}
