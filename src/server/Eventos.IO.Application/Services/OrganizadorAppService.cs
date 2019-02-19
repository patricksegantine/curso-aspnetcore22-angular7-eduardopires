using AutoMapper;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Domain.Organizadores.Commands;
using System;

namespace Eventos.IO.Application.Services
{
    public class OrganizadorAppService : IOrganizadorAppService
    {
        private readonly IMapper _mapper;
        private readonly IOrganizadorRepository _organizadorRepository;
        private readonly IBus _bus;

        public OrganizadorAppService(IMapper mapper, IOrganizadorRepository organizadorRepository, IBus bus)
        {
            _mapper = mapper;
            _organizadorRepository = organizadorRepository;
            _bus = bus;
        }

        public void Registrar(OrganizadorViewModel viewModel)
        {
            var registroCommand = _mapper.Map<RegistrarOrganizadorCommand>(viewModel);
            _bus.SendCommand(registroCommand);
        }

        public void Dispose()
        {
            _organizadorRepository.Dispose();
        }

    }
}
