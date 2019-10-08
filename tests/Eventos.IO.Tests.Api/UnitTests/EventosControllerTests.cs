using AutoMapper;
using Eventos.IO.Api.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Core.Interfaces;
using Eventos.IO.Services.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Eventos.IO.Tests.Api.UnitTests
{
    // AAA => Arrange, Act, Assert

    public class EventosControllerTests
    {
        /*
         * Public Arrange Objects
         */
        public EventosController eventosController;
        public Mock<DomainNotificationHandler> mockNotification;
        public Mock<IMapper> mockMapper;
        public Mock<IMediatorHandler> mockMediator;

        public EventosControllerTests()
        {
            mockMapper = new Mock<IMapper>();
            mockMediator = new Mock<IMediatorHandler>();
            mockNotification = new Mock<DomainNotificationHandler>();

            var mockRepository = new Mock<IEventoRepository>();
            var mockUser = new Mock<IUser>();

            eventosController = new EventosController(
                mockNotification.Object,
                mockMediator.Object,
                mockRepository.Object,
                mockMapper.Object,
                mockUser.Object);
        }

        [Fact]
        public void EventosController_Registrar_RetornarComSucesso()
        {
            // Arrange
            var eventoViewModel = new EventoViewModel();
            var eventoCommand = new RegistrarEventoCommand("Teste", "X", "XXX", 
                System.DateTime.Now, System.DateTime.Now.AddDays(1), true, 0, true, "",
                System.Guid.NewGuid(), System.Guid.NewGuid(),
                new IncluirEnderecoEventoCommand(System.Guid.NewGuid(), "", "", "", "", "", "", "", null));

            // quando vc chamar o Map<xxx>(yyy) retorna o zzz
            mockMapper.Setup(m => m.Map<RegistrarEventoCommand>(eventoViewModel)).Returns(eventoCommand);
            // quando eu pedir GetNotification() retorna a lista vazia 
            mockNotification.Setup(m => m.GetNotifications()).Returns(new List<DomainNotification>());

            // Act
            var result = eventosController.Post(eventoViewModel);

            // Assert
            mockMediator.Verify(m => m.EnviarComando(eventoCommand), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void EventosController_Registrar_RetornarComErrosDeModelState()
        {
            // Arrange
            // mock do método GetNotifications()
            var notificationList = new List<DomainNotification> { new DomainNotification("Erro", "ModelErro") };

            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(m => m.HasNotifications()).Returns(true);

            eventosController.ModelState.AddModelError("Erro","ModelError");

            // Act
            var result = eventosController.Post(new EventoViewModel());

            // Assert
            mockMediator.Verify(m=>m.EnviarComando(It.IsAny<RegistrarEventoCommand>()), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EventosController_Registrar_RetornarComErrosDeDominio()
        {
            // Arrange
            var eventoViewModel = new EventoViewModel();

            var eventoCommand = new RegistrarEventoCommand("Teste com erros de domínio", "X", "XXX",
                System.DateTime.Now, System.DateTime.Now.AddDays(1), true, 0, true, "",
                System.Guid.NewGuid(), System.Guid.NewGuid(),
                new IncluirEnderecoEventoCommand(System.Guid.NewGuid(), "", "", "", "", "", "", "", null));

            mockMapper.Setup(m => m.Map<RegistrarEventoCommand>(eventoViewModel)).Returns(eventoCommand);

            var notificationList = new List<DomainNotification>
            {
                new DomainNotification("Erro","Erro ao adicionar o evento")
            };

            mockNotification.Setup(n => n.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(n => n.HasNotifications()).Returns(true);

            // Act
            var result = eventosController.Post(eventoViewModel);

            // Assert
            mockMediator.Verify(m=>m.EnviarComando(eventoCommand), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}