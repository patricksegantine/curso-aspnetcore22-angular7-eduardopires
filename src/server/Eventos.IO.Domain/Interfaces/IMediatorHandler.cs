using Eventos.IO.Domain.Core.Commands;
using Eventos.IO.Domain.Core.Events;
using System.Threading.Tasks;

namespace Eventos.IO.Domain.Interfaces
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
