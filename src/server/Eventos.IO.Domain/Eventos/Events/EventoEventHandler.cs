using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eventos.IO.Domain.Eventos.Events
{
    public class EventoEventHandler :
        INotificationHandler<EventoRegistradoEvent>,
        INotificationHandler<EventoAtualizadoEvent>,
        INotificationHandler<EventoExcluidoEvent>,
        INotificationHandler<EnderecoEventoIncluidoEvent>,
        INotificationHandler<EnderecoEventoAtualizadoEvent>
    {

        public Task Handle(EventoRegistradoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação

            return Task.CompletedTask;
        }

        public Task Handle(EventoAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação

            return Task.CompletedTask;
        }

        public Task Handle(EventoExcluidoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação

            return Task.CompletedTask;
        }

        public Task Handle(EnderecoEventoIncluidoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação

            return Task.CompletedTask;
        }

        public Task Handle(EnderecoEventoAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação

            return Task.CompletedTask;
        }

    }
}
