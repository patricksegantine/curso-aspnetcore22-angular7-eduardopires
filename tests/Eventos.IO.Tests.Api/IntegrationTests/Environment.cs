using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Eventos.IO.Tests.Api.IntegrationTests
{
    public class Environment
    {
        public static TestServer Server;
        public static HttpClient Client;

        public static void CriarServidor()
        {
            Server = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment("Testing")
                    .UseUrls("http://localhost:8285")
                    .UseStartup<Services.Api.StartupTest>());

            Client = Server.CreateClient();
        }
    }
}