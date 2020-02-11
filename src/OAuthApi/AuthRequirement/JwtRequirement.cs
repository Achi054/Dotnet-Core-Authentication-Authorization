using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace OAuthApi.AuthRequirement
{
    public class JwtRequirement : IAuthorizationRequirement { }

    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public JwtRequirementHandler(IHttpClientFactory client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorize", out var authorizationString))
            {
                var authToken = authorizationString.ToString().Split(' ')[1];

                var response = await _client.GetAsync($"https://localhost:44395/access_token={authToken}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    context.Succeed(requirement);
            }
        }
    }
}
