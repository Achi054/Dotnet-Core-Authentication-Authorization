using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OAuthClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        public IActionResult Index() => View();

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var client1 = _httpClientFactory.CreateClient();
            client1.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response1 = await client1.GetAsync("https://localhost:44319/secret/message");

            await RefreshAuthToken();

            var client2 = _httpClientFactory.CreateClient();
            client2.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response2 = await client2.GetAsync("https://localhost:44319/secret/message");

            return View();
        }

        private async Task RefreshAuthToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var client = _httpClientFactory.CreateClient();
            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44395/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestData),
            };

            var basicCredentials = "username:password";
            var encodedCredentials = Encoding.UTF8.GetBytes(basicCredentials);
            var base64string = Convert.ToBase64String(encodedCredentials);

            request.Headers.Add("Authorization", $"Basic {base64string}");

            await client.SendAsync(request);
        }
    }
}