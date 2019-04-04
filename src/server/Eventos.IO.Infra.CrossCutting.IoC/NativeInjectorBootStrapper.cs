using Eventos.IO.Domain.Core.Handlers;
using Eventos.IO.Domain.Core.Interfaces;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Domain.Organizadores.Commands;
using Eventos.IO.Domain.Organizadores.Events;
using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Eventos.IO.Infra.CrossCutting.Identity.Services;
using Eventos.IO.Infra.Data.Context;
using Eventos.IO.Infra.Data.EventSourcing;
using Eventos.IO.Infra.Data.Repository;
using Eventos.IO.Infra.Data.Repository.EventSourcing;
using Eventos.IO.Infra.Data.UoW;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Eventos.IO.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediatr)
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            #region Domain - Commands

            services.AddScoped<IRequestHandler<RegistrarEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<ExcluirEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<IncluirEnderecoEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarEnderecoEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<RegistrarOrganizadorCommand, bool>, OrganizadorCommandHandler>();

            #endregion

            #region Domain - Events

            // EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();

            // Eventos
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<EventoRegistradoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EventoAtualizadoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EventoExcluidoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EnderecoEventoIncluidoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EnderecoEventoAtualizadoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<OrganizadorRegistradoEvent>, OrganizadorEventHandler>();

            #endregion

            #region Infra - Data

            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IOrganizadorRepository, OrganizadorRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<EventosContext>();
            services.AddScoped<EventStoreSqlContext>();

            #endregion

            #region Infra - Identity

            services.AddTransient<IEmailSender, MessageServices>();
            services.AddTransient<ISmsSender, MessageServices>();
            services.AddScoped<IUser, SignedUser>();

            #endregion

            #region Infra - AspNetFilters

            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<ILogger<GlobalActionLoggerFilter>, Logger<GlobalActionLoggerFilter>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();
            services.AddScoped<GlobalActionLoggerFilter>();

            #endregion
        }
    }
}
