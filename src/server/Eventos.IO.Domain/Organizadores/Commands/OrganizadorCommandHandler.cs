using Eventos.IO.Domain.CommandHandlers;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventos.IO.Domain.Organizadores.Commands
{
    public class OrganizadorCommandHandler : CommandHandler, IHandler<RegistrarOrganizadorCommand>
    {
        private readonly IBus _bus;
        private readonly IOrganizadorRepository _organizadorRepository;

        public OrganizadorCommandHandler(
            IUnitOfWork uow, 
            IBus bus, 
            IDomainNotificationHandler<DomainNotification> notifications,
            IOrganizadorRepository organizadorRepository) : base(uow, bus, notifications)
        {
            _bus = bus;
            _organizadorRepository = organizadorRepository;
        }

        public void Handle(RegistrarOrganizadorCommand message)
        {
            var organizador = new Organizador(message.Id, message.Nome, message.CpfCnpj, message.Email);

            if (!organizador.IsValid())
            {
                NotificarValidacoesErro(organizador.ValidationResult);
                return;
            }

            // Valida cpf e email duplicados
            var organizadorExistente = _organizadorRepository.Buscar(o => o.CpfCnpj == organizador.CpfCnpj || o.Email == organizador.Email);
            if(organizadorExistente.Any())
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "CPF/CNPJ ou e-mail já utilizados"));
                return;
            }

            // inclui no repositório
            _organizadorRepository.Incluir(organizador);

            if (Commit())
            {
                //_bus.RaiseEvent();
            }
        }
    }
}
