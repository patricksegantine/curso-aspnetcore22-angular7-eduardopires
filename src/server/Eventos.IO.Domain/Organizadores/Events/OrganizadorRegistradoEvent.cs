using Eventos.IO.Domain.Core.Events;
using System;

namespace Eventos.IO.Domain.Organizadores.Events
{
    public class OrganizadorRegistradoEvent : Event
    {
        public Guid Id { get; protected set; }

        public string Nome { get; private set; }

        public string CpfCnpj { get; private set; }

        public string Email { get; private set; }

        public OrganizadorRegistradoEvent(Guid id, string nome, string cpfCnpj, string email)
        {
            Id = id;
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
        }

    }
}
