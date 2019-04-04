using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Eventos.IO.Domain.Core.Interfaces
{
    // Interface para representar o usuário conectado na aplicação (independente o Identity)
    public interface IUser
    {
        string Name { get; }

        bool IsAuthenticated();

        Guid GetUserId();

        IEnumerable<Claim> GetClaimsIdentity();
    }
}
