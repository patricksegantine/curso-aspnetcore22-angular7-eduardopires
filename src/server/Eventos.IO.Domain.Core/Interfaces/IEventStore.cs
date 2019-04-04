using Eventos.IO.Domain.Core.Events;

namespace Eventos.IO.Domain.Core.Interfaces
{
    public interface IEventStore
    {
        void SalvarEvento<T>(T evento) where T : Event;
    }
}
