using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OAuthClient.Constants;

namespace OAuthClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(configureOptions =>
                    {
                        configureOptions.DefaultAuthenticateScheme = "ClientCookie";
                        configureOptions.DefaultSignInScheme = "ClientCookie";
                        configureOptions.DefaultChallengeScheme = "OAuthServer";
                    })
                    .AddCookie("ClientCookie")
                    .AddOAuth("OAuthServer", configureOptions =>
                    {
                        configureOptions.CallbackPath = "/oauth/callback";
                        configureOptions.ClientId = TokenConstants.ClientId;
                        configureOptions.ClientSecret = TokenConstants.ClientKey;
                        configureOptions.AuthorizationEndpoint = $"{TokenConstants.Issuer}oauth/authorize";
                        configureOptions.TokenEndpoint = $"{TokenConstants.Issuer}oauth/token";

                        configureOptions.SaveTokens = true;

                        configureOptions.Events = new OAuthEvents()
                        {
                            OnCreatingTicket = context =>
                            {
                                var accessToken = context.AccessToken;
                                var base64Token = accessToken.Split('.')[1];
                                var claims = JsonConvert.DeserializeObject<IReadOnlyDictionary<string, string>>(base64Token);
                                foreach (var claim in claims)
                                    context.Identity.AddClaim(new Claim(claim.Key, claim.Value));

                                return Task.CompletedTask;
                            }
                        };
                    });

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
