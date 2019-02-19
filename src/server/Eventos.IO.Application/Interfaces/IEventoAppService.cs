using Eventos.IO.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Application.Interfaces
{
    public interface IEventoAppService : IDisposable
    {
        void Registrar(EventoViewModel viewModel);

        EventoViewModel ObterPorId(Guid id);

        IEnumerable<EventoViewModel> ObterTodos();

        IEnumerable<EventoViewModel> ObterEventosPorOrganizador(Guid organizadorId);

        void Atualizar(EventoViewModel viewModel);

        void Excluir(Guid id);

        void IncluirEndereco(EnderecoViewModel enderecoViewModel);

        void AtualizarEndereco(EnderecoViewModel enderecoViewModel);

        EnderecoViewModel ObterEnderecoPorId(Guid id);
    }
}
