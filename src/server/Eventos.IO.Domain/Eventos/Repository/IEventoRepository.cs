using Eventos.IO.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos.Repository
{
    public interface IEventoRepository : IRepository<Evento>
    {
        // Posso colocar demais métodos especializados
        // ObterEnderecoPorEvento
        // ObterEventoPorCidade

        Evento ObterMeuEventoPorId(Guid id, Guid organizadorId);

        IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId);

        Endereco ObterEnderecoPorId(Guid id);

        void IncluirEndereco(Endereco endereco);

        void AtualizarEndereco(Endereco endereco);

        IEnumerable<Categoria> ObterCategorias();
    }
}
