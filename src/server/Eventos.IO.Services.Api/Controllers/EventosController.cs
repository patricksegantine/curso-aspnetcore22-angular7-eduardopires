using AutoMapper;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Services.Api.Controllers
{
    public class EventosController : BaseController
    {
        private readonly IBus _bus;
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;

        // aula 19 - 2h51min
        public EventosController(
            IDomainNotificationHandler<DomainNotification> notifications,
            IBus bus,
            IUser user, 
            IEventoRepository eventoRepository, //IEventoAppService eventoAppService,
            IMapper mapper) : base(notifications, bus, user)
        {
            _bus = bus;
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos")]
        public IEnumerable<Evento> ObterTodos()
        {
            // Se a camada de apresentação for somente 
            // em SPA, NÃO USAR  a camada de Application 
            // e mover as responsabilidades para a Controller
            return _eventoRepository.ObterTodos();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos/{id:guid}")]
        public Evento Get(Guid id, int version)
        {
            return _eventoRepository.ObterPorId(id);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos/categorias")]
        public IEnumerable<Categoria> ObterCategorias()
        {
            // Caso estive usando a camada Application
            //return _mapper.Map<IEnumerable<CategoriaViewModel>>(_eventoRepository.ObterCategorias());

            return _eventoRepository.ObterCategorias();
        }

        [HttpPost]
        [Route("eventos")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Post([FromBody]RegistrarEventoCommand eventoCommand)
        {
            if (!ModelState.IsValid)
            {
                NotificarErroModelStateInvalido();
                return Response();
            }

            // Registra o evento
            _bus.SendCommand(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpPut]
        [Route("eventos")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Put([FromBody]AtualizarEventoCommand eventoCommand)
        {
            // Atualiza o evento

            // Caso estive usando a camada Application
            //_eventoRepository.Atualizar(eventoCommand);

            _bus.SendCommand(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpDelete]
        [Route("eventos/{id:guid}")]
        [Authorize(Policy = "PodeExcluir")]
        public IActionResult Delete(Guid id)
        {
            _bus.SendCommand(new ExcluirEventoCommand(id));
            return Response();
        }
    }
}
