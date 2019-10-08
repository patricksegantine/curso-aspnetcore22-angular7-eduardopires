using AutoMapper;
using Eventos.IO.Api.ViewModels;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Organizadores;

namespace Eventos.IO.Services.Api.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Evento, EventoViewModel>();
            CreateMap<Endereco, EnderecoViewModel>();
            CreateMap<Categoria, CategoriaViewModel>();
            CreateMap<Organizador, OrganizadorViewModel>();
        }
    }

}
