using System.Threading.Tasks;

using Eventos.IO.Domain.Core.Commands;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Interfaces;
using Eventos.IO.Domain.Core.Notifications;

using MediatR;

namespace Eventos.IO.Domain.Core.Handlers
{
    /// <summary>
    /// In Memory Bus
    /// </summary>
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public MediatorHandler(IMediator mediator, IEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        /// <summary>
        /// Send an Command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task EnviarComando<T>(T command) where T : Command
        {
            await _mediator.Send(command);
        }

        /// <summary>
        /// Raise an Event
        /// </summary>
        /// <typeparam name="T">Pode ser um Event ou DomainNotification de erros</typeparam>
        /// <param name="evento"></param>
        /// <returns></returns>
        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            // Aplicação do EventSourcing Pattern 
            // Se a mensagem não for um DomainNotification, salva o evento no banco de dados
            if (!evento.MessageType.Equals(nameof(DomainNotification)))
                _eventStore?.SalvarEvento(evento);

            await _mediator.Publish(evento);
        }
    }
}
