using System;
using System.Threading;
using System.Threading.Tasks;

using Eventos.IO.Domain.Core.CommandHandlers;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Core.Interfaces;

using MediatR;

namespace Eventos.IO.Domain.Eventos.Commands
{
    public class EventoCommandHandler : CommandHandler,
        IRequestHandler<RegistrarEventoCommand, bool>,
        IRequestHandler<AtualizarEventoCommand, bool>,
        IRequestHandler<ExcluirEventoCommand, bool>,
        IRequestHandler<IncluirEnderecoEventoCommand, bool>,
        IRequestHandler<AtualizarEnderecoEventoCommand, bool>
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IMediatorHandler _mediator;
        private readonly IUser _user;

        public EventoCommandHandler(IEventoRepository eventoRepository,
                                    IUnitOfWork uow,
                                    INotificationHandler<DomainNotification> notifications,
                                    IMediatorHandler mediator,
                                    IUser user)
            : base(uow, mediator, notifications)
        {
            _eventoRepository = eventoRepository;
            _mediator = mediator;
            _user = user;
        }

        public Task<bool> Handle(RegistrarEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Endereco.Id, message.Endereco.Logradouro, message.Endereco.Numero, message.Endereco.Complemento, message.Endereco.Bairro, message.Endereco.CEP, message.Endereco.Cidade, message.Endereco.Estado, message.Endereco.EventoId.Value);

            var evento = Evento.EventoFactory.NovoEvento(message.Id, message.Nome, message.DescricaoCurta, message.DescricaoLonga,
                message.DataInicio, message.DataFim, message.Gratuito, message.Valor,
                message.Online, message.NomeEmpresa, message.OrganizadorId, endereco, message.CategoriaId);

            if (!EventoValido(evento)) return Task.FromResult(false);

            _eventoRepository.Incluir(evento);

            if (Commit())
            {
                _mediator.RaiseEvent(new EventoRegistradoEvent(evento.Id, evento.Nome, evento.DataInicio, evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(AtualizarEventoCommand message, CancellationToken cancellationToken)
        {
            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (!EventoExiste(message.Id, message.MessageType)) return Task.FromResult(false);

            if (eventoAtual.OrganizadorId != _user.GetUserId()) // Recupera o usuário logado
            {
                _mediator.RaiseEvent(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador!"));
                return Task.FromResult(false);
            }

            var evento = Evento.EventoFactory.NovoEvento(message.Id, message.Nome, message.DescricaoCurta, message.DescricaoLonga,
                message.DataInicio, message.DataFim, message.Gratuito, message.Valor,
                message.Online, message.NomeEmpresa, message.OrganizadorId, eventoAtual.Endereco, message.CategoriaId);

            if (!evento.Online && evento.Endereco == null)
            {
                _mediator.RaiseEvent(new DomainNotification(message.MessageType, "Não é possível atualizar um evento presencial sem informar o endereço!"));
                return Task.FromResult(false);
            }

            if (!EventoValido(evento)) return Task.FromResult(false);

            _eventoRepository.Atualizar(evento);

            if (Commit())
            {
                _mediator.RaiseEvent(new EventoAtualizadoEvent(evento.Id, evento.Nome));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(ExcluirEventoCommand message, CancellationToken cancellationToken)
        {
            if (!EventoExiste(message.Id, message.MessageType))
                return Task.FromResult(false);

            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (eventoAtual.OrganizadorId != _user.GetUserId())
            {
                _mediator.RaiseEvent(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador!"));
                return Task.FromResult(false);
            }

            // Validações de negócio
            eventoAtual.ExcluirEvento(); // Exclusão lógica

            _eventoRepository.Atualizar(eventoAtual);

            if (Commit())
            {
                _mediator.RaiseEvent(new EventoExcluidoEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(IncluirEnderecoEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Id, message.CEP, message.Logradouro, message.Numero, message.Complemento,
                message.Bairro, message.Cidade, message.Estado, message.EventoId.Value);

            if (!endereco.EhValido())
            {
                NotifyValidationErrors(endereco.ValidationResult);
                return Task.FromResult(false);
            }

            _eventoRepository.IncluirEndereco(endereco);

            if (Commit())
            {
                _mediator.RaiseEvent(new EnderecoEventoIncluidoEvent(message.Id, message.CEP, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.Cidade, message.Estado, message.EventoId.Value));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(AtualizarEnderecoEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Id, message.CEP, message.Logradouro, message.Numero, message.Complemento,
                message.Bairro, message.Cidade, message.Estado, message.EventoId.Value);

            if (!endereco.EhValido())
            {
                NotifyValidationErrors(endereco.ValidationResult);
                return Task.FromResult(false);
            }

            _eventoRepository.IncluirEndereco(endereco);

            if (Commit())
            {
                // TODO: _bus.RaiseEvent(new EnderecoEventoAtualizadoEvent());
            }

            return Task.FromResult(true);
        }

        // Clean Code... singularidade
        private bool EventoValido(Evento evento)
        {
            if (evento.EhValido()) return true;

            NotifyValidationErrors(evento.ValidationResult);
            return false;
        }

        private bool EventoExiste(Guid id, string messageType)
        {
            var evento = _eventoRepository.ObterPorId(id);

            if (evento != null) return true;

            _mediator.RaiseEvent(new DomainNotification(messageType, "Evento não encontrado."));
            return false;
        }

    }
}
