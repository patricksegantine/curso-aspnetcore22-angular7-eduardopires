using Eventos.IO.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos
{
    public class Categoria : Entity<Categoria>
    {
        protected Categoria() { }

        public Categoria(Guid id)
        {
            Id = id;
        }

        public string Nome { get; private set; }

        // EF Propriedade de Navegação
        public virtual ICollection<Evento> Eventos { get; set; }

        public override bool EhValido()
        {
            return true;
        }
    }
}