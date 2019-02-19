using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Eventos;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Organizadores
{
    public class Organizador : Entity<Organizador>
    {
        protected Organizador() { }

        public Organizador(Guid id, string nome, string cpfCnpj, string email)
        {
            Id = id;
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
        }

        public string Nome { get; private set; }

        public string CpfCnpj { get; private set; }

        public string Email { get; private set; }

        // EF Propriedade de Navegação
        public virtual ICollection<Evento> Eventos { get; private set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}