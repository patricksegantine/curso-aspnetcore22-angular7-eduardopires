using AutoMapper;

namespace Eventos.IO.Application.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(profilers =>
            {
                profilers.AddProfile(new DomainToViewModelMappingProfile());
                profilers.AddProfile(new ViewModelToDomainMappingProfile());
            });
        }
    }
}
