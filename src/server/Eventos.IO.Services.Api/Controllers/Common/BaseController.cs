using System;
using System.Linq;

using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Core.Interfaces;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eventos.IO.Services.Api.Controllers.Common
{
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;

        protected Guid OrganizadorId { get; set; }

        protected BaseController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            IUser user)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;

            if (user.IsAuthenticated())
            {
                OrganizadorId = user.GetUserId();
            }
        }

        /// <summary>
        /// Objeto de resposta às requisições
        /// </summary>
        /// <param name="result">Qualquer objeto de retorno</param>
        /// <returns></returns>
        protected new IActionResult Response(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                erros = _notifications.GetNotifications().Select(n => n.Value) // retorna somente o valor da notificação
            });
        }

        /// <summary>
        /// Verifica se ocorrou algum erro de Domínio
        /// </summary>
        /// <returns></returns>
        protected bool OperacaoValida()
        {
            return !_notifications.HasNotifications();
        }

        /// <summary>
        /// Notifica os erros de estado de modelos
        /// </summary>
        protected void NotificarErroModelStateInvalido()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(string.Empty, erroMsg);
            }
        }

        /// <summary>
        /// Notifica os erros ocorridos no Domínio
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="mensagem"></param>
        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediator.PublicarEvento(new DomainNotification(codigo, mensagem));
        }
    }
}