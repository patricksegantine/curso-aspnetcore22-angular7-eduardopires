using Eventos.IO.Domain.Core.Events;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Infra.Data.Repository.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent @event);
        IList<StoredEvent> All(Guid aggregatedId);
    }
}
