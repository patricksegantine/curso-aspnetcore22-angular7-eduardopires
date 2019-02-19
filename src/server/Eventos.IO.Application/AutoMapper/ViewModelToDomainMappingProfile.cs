using AutoMapper;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Organizadores.Commands;

namespace Eventos.IO.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            #region Evento

            CreateMap<EventoViewModel, RegistrarEventoCommand>()
                .ConstructUsing(vm=>new RegistrarEventoCommand(vm.Nome, vm.DescricaoCurta, vm.DescricaoLonga, vm.DataInicio, vm.DataFim, vm.Gratuito, vm.Valor, vm.Online, vm.NomeEmpresa, vm.OrganizadorId, vm.CategoriaId,
                    new IncluirEnderecoEventoCommand(vm.Endereco.Id, vm.Endereco.Logradouro, vm.Endereco.Numero, vm.Endereco.Complemento, vm.Endereco.Bairro, vm.Endereco.CEP, vm.Endereco.Cidade, vm.Endereco.Estado, vm.Id)));

            CreateMap<EventoViewModel, AtualizarEventoCommand>()
                .ConstructUsing(vm => new AtualizarEventoCommand(vm.Id, vm.Nome, vm.DescricaoCurta, vm.DescricaoLonga, vm.DataInicio, vm.DataFim, vm.Gratuito, vm.Valor, vm.Online, vm.NomeEmpresa, vm.OrganizadorId, vm.CategoriaId));

            CreateMap<EventoViewModel, ExcluirEventoCommand>()
                .ConstructUsing(vm => new ExcluirEventoCommand(vm.Id));

            #region Endereço do Evento

            CreateMap<EnderecoViewModel, IncluirEnderecoEventoCommand>()
                .ConstructUsing(vm => new IncluirEnderecoEventoCommand(vm.Id, vm.CEP, vm.Logradouro, vm.Numero, vm.Complemento, vm.Bairro, vm.Cidade, vm.Estado, vm.EventoId));

            CreateMap<EnderecoViewModel, AtualizarEnderecoEventoCommand>()
                .ConstructUsing(vm => new AtualizarEnderecoEventoCommand(vm.Id, vm.CEP, vm.Logradouro, vm.Numero, vm.Complemento, vm.Bairro, vm.Cidade, vm.Estado, vm.EventoId));
            
            #endregion 

            #endregion

            #region Organizador

            CreateMap<OrganizadorViewModel, RegistrarOrganizadorCommand>()
                .ConstructUsing(vm => new RegistrarOrganizadorCommand(vm.Id, vm.Nome, vm.CpfCnpj, vm.Email));

            #endregion 
        }
    }

}
