using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
            => this.httpClientFactory = httpClientFactory;

        public IActionResult Index() => View();

        public IActionResult Logout() => SignOut("authCookie", "oidc");

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // retrieve data
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);
            var apiResponse = await apiClient.GetAsync("https://localhost:44311/message");
            var message = await apiResponse.Content.ReadAsStringAsync();

            await RefreshToken();

            return View();
        }

        private async Task RefreshToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var serverClient = httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44326/");
            var tokenResponse = await serverClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    RefreshToken = refreshToken,
                    ClientId = "054_mvc",
                    ClientSecret = "sujith_acharya_mvc"
                });

            var authInfo = await HttpContext.AuthenticateAsync("authCookie");
            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await HttpContext.SignInAsync("authCookie", authInfo.Principal, authInfo.Properties);
        }
    }
}
