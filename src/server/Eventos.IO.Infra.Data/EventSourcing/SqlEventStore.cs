using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Interfaces;
using Eventos.IO.Infra.Data.Repository.EventSourcing;
using System.Text.Json;

namespace Eventos.IO.Infra.Data.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IUser _user;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, IUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void SalvarEvento<T>(T evento) where T : Event
        {
            var serializedData = JsonSerializer.Serialize(evento);
            var storedEvent = new StoredEvent(
                evento,
                serializedData,
                _user.GetUserId().ToString());

            _eventStoreRepository.Store(storedEvent);
        }
    }
}
