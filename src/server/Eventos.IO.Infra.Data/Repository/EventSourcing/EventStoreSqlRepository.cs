using System;
using System.Collections.Generic;
using System.Linq;

using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Infra.Data.Context;

namespace Eventos.IO.Infra.Data.Repository.EventSourcing
{
    public class EventStoreSqlRepository : IEventStoreRepository
    {
        private readonly EventStoreSqlContext _context;

        public EventStoreSqlRepository(EventStoreSqlContext context)
        {
            _context = context;
        }

        public void Store(StoredEvent @event)
        {
            _context.StoredEvents.Add(@event);
            _context.SaveChanges();
        }

        public IList<StoredEvent> All(Guid aggregatedId)
        {
            return (from e in _context.StoredEvents where e.AggregatedId == aggregatedId select e).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
