using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Eventos.IO.Tests.Api.IntegrationTests.DTO;
using Newtonsoft.Json;
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
                Nome = "Raquel C M Segantine",
                CpfCnpj = "12345678900",
                Email = "raquelcms@outlook.com",
                Password = "Rcms1234*",
                ConfirmPassword = "Rcms1234*"
            };

            // Act
            var postContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await Environment.Client.PostAsync("api/v1/nova-conta", postContent);
            var conteudo = response.Content.ReadAsStringAsync().Result;
            //var usuario = JsonConvert.DeserializeObject<UsuarioDTO>(conteudo);

            // Assert
            //response.EnsureSuccessStatusCode();
            //var token = usuario.;
            //Assert.NotEmpty(token);
        }
    }
}