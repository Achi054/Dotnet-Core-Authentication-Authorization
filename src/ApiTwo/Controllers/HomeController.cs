using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
            => this.httpClientFactory = httpClientFactory;

        [Route("/index")]
        public async Task<IActionResult> Index()
        {
            // retrieve Token
            var serverClient = httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44326/");

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "054",
                ClientSecret = "sujith_acharya",
                Scope = "ApiOne"
            };
            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(tokenRequest);

            // retrieve data
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var apiResponse = await apiClient.GetAsync("https://localhost:44311/message");

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = await apiResponse.Content.ReadAsStringAsync()
            });
        }
    }
}
