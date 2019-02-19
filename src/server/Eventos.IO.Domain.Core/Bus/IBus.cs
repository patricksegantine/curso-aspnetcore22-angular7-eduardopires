using Eventos.IO.Domain.Core.Commands;
using Eventos.IO.Domain.Core.Events;

namespace Eventos.IO.Domain.Core.Bus
{
    // Disparar Commands e Events
    public interface IBus
    {
        // Um Command é sempre enviado de uma camada superior
        void SendCommand<T>(T theCommand) where T : Command;

        // Um Event é sempre lançado pra que faça algum efeito: notificação, persistência etc.
        void RaiseEvent<T>(T theEvent) where T : Event;
    }
}
