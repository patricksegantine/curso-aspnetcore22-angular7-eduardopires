using Eventos.IO.Domain.Core.Commands;
using System;

namespace Eventos.IO.Domain.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        CommandResponse Commit();
    }
}
