using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Suriscode_API;
using Suriscode_API.Models;

namespace SuriscodeApiTest
{
    public class PedidoTests
    {
#pragma warning disable NUnit1032
        private HttpClient _client;
#pragma warning restore NUnit1032 

        [SetUp]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task PedidoOk()
        {
            var pedido = new PedidoQueryDto
            {
                VendedorId = 1,
                ArticuloIds = ["K1020"]
            };

            var response = await _client.PostAsJsonAsync("/pedido", pedido);
            var result = await response.Content.ReadAsStringAsync();
            Assert.That(result, Is.EqualTo(("\"¡El pedido fue generado con éxito!\"")));
        }
    }
}