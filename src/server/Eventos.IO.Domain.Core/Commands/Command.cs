using Eventos.IO.Domain.Core.Events;
using MediatR;
using System;

namespace Eventos.IO.Domain.Core.Commands
{
    public class Command : Message, INotification
    {
        public DateTime Timestamp { get; set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
