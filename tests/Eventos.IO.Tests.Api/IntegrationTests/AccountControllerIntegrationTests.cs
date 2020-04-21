using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Eventos.IO.Tests.Api.IntegrationTests.DTO;
using Xunit;

namespace Eventos.IO.Tests.Api.IntegrationTests
{
    public class AccountControllerIntegrationTests
    {
        public AccountControllerIntegrationTests()
        {
             Environment.CriarServidor();   
        }

        [Fact]
        public async Task AccountController_RegistrarNovoOrganizador_RetornarComSucesso()
        {
            // Arrange
            var user = new RegisterViewModel
            {
                Nome = "Patrick Segantine",
                CpfCnpj = "00123456789",
                Email = "patrick@email.com",
                Password = "PSeg1234*",
                ConfirmPassword = "PSeg1234*"
            };

            // Act
            var postContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var response = await Environment.Client.PostAsync("api/v1/Account/nova-conta", postContent);
            var usuarioResult = JsonSerializer.Deserialize<UsuarioJsonDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.EnsureSuccessStatusCode();
            //var token = usuarioResult.data.result;
            //Assert.NotEmpty(token);
        }
    }
}