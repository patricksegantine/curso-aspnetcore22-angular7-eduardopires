using Eventos.IO.Domain.CommandHandlers;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using System;

namespace Eventos.IO.Domain.Eventos.Commands
{
    public class EventoCommandHandler : CommandHandler,
        IHandler<RegistrarEventoCommand>,
        IHandler<AtualizarEventoCommand>,
        IHandler<ExcluirEventoCommand>,
        IHandler<IncluirEnderecoEventoCommand>,
        IHandler<AtualizarEnderecoEventoCommand>
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IBus _bus;
        private readonly IUser _user;

        public EventoCommandHandler(IEventoRepository eventoRepository,
                                    IUnitOfWork uow,
                                    IBus bus,
                                    IDomainNotificationHandler<DomainNotification> notifications,
                                    IUser user)
            : base(uow, bus, notifications)
        {
            _eventoRepository = eventoRepository;
            _bus = bus;
            _user = user;
        }

        public void Handle(RegistrarEventoCommand message)
        {
            var endereco = new Endereco(message.Endereco.Id, message.Endereco.Logradouro, message.Endereco.Numero, message.Endereco.Complemento, message.Endereco.Bairro, message.Endereco.CEP, message.Endereco.Cidade, message.Endereco.Estado, message.Endereco.EventoId.Value);

            var evento = Evento.EventoFactory.NovoEvento(message.Id, message.Nome, message.DescricaoCurta, message.DescricaoLonga,
                message.DataInicio, message.DataFim, message.Gratuito, message.Valor, 
                message.Online, message.NomeEmpresa, message.OrganizadorId, endereco, message.CategoriaId);

            if (!EventoValido(evento)) return;

            _eventoRepository.Incluir(evento);

            if (Commit())
            {
                _bus.RaiseEvent(new EventoRegistradoEvent(evento.Id, evento.Nome, evento.DataInicio, evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
            }
        }

        public void Handle(AtualizarEventoCommand message)
        {
            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (!EventoExiste(message.Id, message.MessageType)) return;

            if (eventoAtual.OrganizadorId != _user.GetUserId()) // Recupera o usuário logado
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador!"));
                return;
            }

            var evento = Evento.EventoFactory.NovoEvento(message.Id, message.Nome, message.DescricaoCurta, message.DescricaoLonga,
                message.DataInicio, message.DataFim, message.Gratuito, message.Valor, 
                message.Online, message.NomeEmpresa, message.OrganizadorId, eventoAtual.Endereco, message.CategoriaId);

            if(!evento.Online && evento.Endereco == null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Não é possível atualizar um evento presencial sem informar o endereço!"));
                return;
            }

            if (!EventoValido(evento)) return;

            _eventoRepository.Atualizar(evento);

            if (Commit())
            {
                _bus.RaiseEvent(new EventoAtualizadoEvent(evento.Id, evento.Nome));
            }
        }

        public void Handle(ExcluirEventoCommand message)
        {
            if (!EventoExiste(message.Id, message.MessageType)) return;

            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (eventoAtual.OrganizadorId != _user.GetUserId())
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador!"));
                return;
            }

            // Validações de negócio
            eventoAtual.ExcluirEvento(); // Exclusão lógica

            _eventoRepository.Atualizar(eventoAtual);

            if (Commit())
            {
                _bus.RaiseEvent(new EventoExcluidoEvent(message.Id));
            }
        }

        public void Handle(IncluirEnderecoEventoCommand message)
        {
            var endereco = new Endereco(message.Id, message.CEP, message.Logradouro, message.Numero, message.Complemento,
                message.Bairro, message.Cidade, message.Estado, message.EventoId.Value);

            if (!endereco.IsValid())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return;
            }

            _eventoRepository.IncluirEndereco(endereco);

            if (Commit())
            {
                // TODO: _bus.RaiseEvent(new EnderecoEventoIncluidoEvent());
            }
        }

        public void Handle(AtualizarEnderecoEventoCommand message)
        {
            var endereco = new Endereco(message.Id, message.CEP, message.Logradouro, message.Numero, message.Complemento,
                message.Bairro, message.Cidade, message.Estado, message.EventoId.Value);

            if (!endereco.IsValid())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return;
            }

            _eventoRepository.IncluirEndereco(endereco);

            if (Commit())
            {
                // TODO: _bus.RaiseEvent(new EnderecoEventoAtualizadoEvent());
            }
        }


        // Clean Code... singularidade
        private bool EventoValido(Evento evento)
        {
            if (evento.IsValid()) return true;

            NotificarValidacoesErro(evento.ValidationResult);
            return false;
        }

        private bool EventoExiste(Guid id, string messageType)
        {
            var evento = _eventoRepository.ObterPorId(id);

            if (evento != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Evento não encontrado."));
            return false;
        }


    }
}
