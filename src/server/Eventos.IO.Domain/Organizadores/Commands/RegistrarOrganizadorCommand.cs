using Eventos.IO.Domain.Core.Commands;
using System;

namespace Eventos.IO.Domain.Organizadores.Commands
{
    public class RegistrarOrganizadorCommand : Command
    {
        public Guid Id { get; protected set; }

        public string Nome { get; private set; }

        public string CpfCnpj { get; private set; }

        public string Email { get; private set; }

        public RegistrarOrganizadorCommand(Guid id, string nome, string cpfCnpj, string email)
        {
            Id = id;
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
        }
    }
}
