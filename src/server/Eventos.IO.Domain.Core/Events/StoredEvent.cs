using System;

namespace Eventos.IO.Domain.Core.Events
{
    public class StoredEvent : Event
    {
        // EF Constructor
        protected StoredEvent() { }

        public StoredEvent(Event evento, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregatedId = evento.AggregatedId;
            MessageType = evento.MessageType;
            Data = data;
            User = user;
        }

        public Guid Id { get; private set; }

        public string Data { get; private set; }

        public string User { get; private set; }
    }
}
