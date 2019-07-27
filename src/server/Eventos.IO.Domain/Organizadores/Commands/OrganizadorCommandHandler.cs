using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Eventos.IO.Domain.Core.CommandHandlers;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Core.Interfaces;
using Eventos.IO.Domain.Organizadores.Events;

using MediatR;

namespace Eventos.IO.Domain.Organizadores.Commands
{
    public class OrganizadorCommandHandler : CommandHandler,
        IRequestHandler<RegistrarOrganizadorCommand, bool>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IOrganizadorRepository _organizadorRepository;

        public OrganizadorCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IOrganizadorRepository organizadorRepository) : base(uow, mediator, notifications)
        {
            _mediator = mediator;
            _organizadorRepository = organizadorRepository;
        }

        public Task<bool> Handle(RegistrarOrganizadorCommand message, CancellationToken cancellationToken)
        {
            var organizador = new Organizador(message.Id, message.Nome, message.CpfCnpj, message.Email);

            if (!organizador.EhValido())
            {
                NotifyValidationErrors(organizador.ValidationResult);
                return Task.FromResult(false);
            }

            // Valida cpf e email duplicados
            var organizadorExistente = _organizadorRepository.Buscar(o => o.CpfCnpj == organizador.CpfCnpj || o.Email == organizador.Email);
            if (organizadorExistente.Any())
            {
                _mediator.RaiseEvent(new DomainNotification(message.MessageType, "CPF/CNPJ ou e-mail já utilizados"));
                return Task.FromResult(false);
            }

            // inclui no repositório
            _organizadorRepository.Incluir(organizador);

            if (Commit())
            {
                _mediator.RaiseEvent(new OrganizadorRegistradoEvent(organizador.Id, organizador.Nome, organizador.CpfCnpj, organizador.Email));
            }

            return Task.FromResult(true);
        }
    }
}
