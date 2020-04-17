using System;
using System.Collections.Generic;

using Eventos.IO.Api.ViewModels;
using Eventos.IO.Domain.Core.Interfaces;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Repository;

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eventos.IO.Services.Api.Controllers.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Eventos.IO.Services.Api.Controllers
{
    public class EventosController : BaseController
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediator;

        public EventosController(INotificationHandler<DomainNotification> notifications,
                                 IMediatorHandler mediator,
                                 IEventoRepository eventoRepository,
                                 IMapper mapper,
                                 IUser user, 
                                 IMemoryCache cache
                                 ) : base(notifications, mediator, user)
        {
            _mediator = mediator;
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos")]
        public IEnumerable<EventoViewModel> Get()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {

            }
                .SetSlidingExpiration(TimeSpan.FromSeconds(3));


            return _mapper.Map<IEnumerable<EventoViewModel>>(_eventoRepository.ObterTodos());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos/{id:guid}")]
        public EventoViewModel Get(Guid id, int version)
        {
            return _mapper.Map<EventoViewModel>(_eventoRepository.ObterPorId(id));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos/categorias")]
        public IEnumerable<CategoriaViewModel> ObterCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaViewModel>>(_eventoRepository.ObterCategorias());
        }

        [HttpGet]
        [Authorize(Policy = "PodeConsultar")]
        [Route("eventos/meus-eventos")]
        public IEnumerable<EventoViewModel> ObterMeusEventos()
        {
            // NOTA: OrganizadorId está na classe base e 
            // é recuperado pelo TOKEN
            return _mapper.Map<IEnumerable<EventoViewModel>>(_eventoRepository.ObterEventoPorOrganizador(OrganizadorId));
        }

        [HttpGet]
        [Authorize(Policy = "PodeConsultar")]
        [Route("eventos/meus-eventos/{id:guid}")]
        public IActionResult ObterMeuEventoPorId(Guid id)
        {
            var evento = _mapper.Map<EventoViewModel>(_eventoRepository.ObterMeuEventoPorId(id, OrganizadorId));
            return evento == null ? StatusCode(404) : Response(evento);
        }

        [HttpPost]
        [Route("eventos")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Post(EventoViewModel eventoViewModel)
        {
            // o atributo ApiController veio para simplificar a codificação de Controllers...
            // 1) O parâmetro eventoViewModel não foi marcado com FromBody, já que 
            // a presença do atributo ApiController torna possível inferir que 
            // o conteúdo associado a este elemento se encontra no corpo de uma requisição;
            // 2) A instrução que checa o conteúdo da propriedade IsValid no objeto 
            // ModelState também foi removida. Qualquer inconsistência decorrente 
            // de violação das regras nas Data Annotations da classe Comentario irá 
            // resultar na geração automática de um erro do tipo 400 (Bad Request).
            if (!ModelState.IsValid)
            {
                NotificarErroModelStateInvalido();
                return Response();
            }

            var eventoCommand = _mapper.Map<RegistrarEventoCommand>(eventoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpPut]
        [Route("eventos")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Put(EventoViewModel eventoViewModel)
        {
            var eventoCommand = _mapper.Map<AtualizarEventoCommand>(eventoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpDelete]
        [Route("eventos/{id:guid}")]
        [Authorize(Policy = "PodeExcluir")]
        public IActionResult Delete(Guid id)
        {
            var eventoCommand = new ExcluirEventoCommand(id);

            _mediator.EnviarComando(new ExcluirEventoCommand(id));
            return Response();
        }

        [HttpPost]
        [Authorize(Policy = "PodeGravar")]
        [Route("endereco")]
        public IActionResult Post(EnderecoViewModel enderecoViewModel)
        {
            var eventoCommand = _mapper.Map<IncluirEnderecoEventoCommand>(enderecoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpPut]
        [Authorize(Policy = "PodeGravar")]
        [Route("endereco")]
        public IActionResult Put(EnderecoViewModel enderecoViewModel)
        {
            var eventoCommand = _mapper.Map<AtualizarEnderecoEventoCommand>(enderecoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }
    }
}
