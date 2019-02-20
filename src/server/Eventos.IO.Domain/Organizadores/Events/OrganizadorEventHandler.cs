using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eventos.IO.Domain.Organizadores.Events
{
    public class OrganizadorEventHandler : INotificationHandler<OrganizadorRegistradoEvent>
    {
        public Task Handle(OrganizadorRegistradoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Enviar um email?

            return Task.CompletedTask;
        }
    }
}
