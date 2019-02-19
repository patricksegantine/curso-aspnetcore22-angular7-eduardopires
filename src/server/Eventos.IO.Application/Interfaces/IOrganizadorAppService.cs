using Eventos.IO.Application.ViewModels;
using System;

namespace Eventos.IO.Application.Interfaces
{
    public interface IOrganizadorAppService : IDisposable
    {
        void Registrar(OrganizadorViewModel viewModel);
    }
}
