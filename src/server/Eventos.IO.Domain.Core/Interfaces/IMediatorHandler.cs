using Eventos.IO.Domain.Core.Commands;
using Eventos.IO.Domain.Core.Events;
using System.Threading.Tasks;

namespace Eventos.IO.Domain.Core.Interfaces
{
    public interface IMediatorHandler
    {
        Task EnviarComando<T>(T command) where T : Command;
        Task PublicarEvento<T>(T @event) where T : Event;
    }
}
