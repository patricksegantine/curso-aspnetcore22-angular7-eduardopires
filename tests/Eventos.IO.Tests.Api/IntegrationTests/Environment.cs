using System.IO;
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
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseUrls("http://localhost:8285")
                    .UseStartup<Services.Api.Startup>());

            Client = Server.CreateClient();
        }
    }
}