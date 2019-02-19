using Eventos.IO.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Domain.Eventos.Events
{
    public class EventoEventHandler :
        IHandler<EventoRegistradoEvent>,
        IHandler<EventoAtualizadoEvent>,
        IHandler<EventoExcluidoEvent>,
        IHandler<EnderecoEventoIncluidoEvent>,
        IHandler<EnderecoEventoAtualizadoEvent>
    {
        public void Handle(EventoRegistradoEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(EventoAtualizadoEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(EventoExcluidoEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(EnderecoEventoIncluidoEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(EnderecoEventoAtualizadoEvent message)
        {
            throw new NotImplementedException();
        }

    }
}
