using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpClient();

            services.AddAuthentication(configureOptions =>
                    {
                        configureOptions.DefaultScheme = "authCookie";
                        configureOptions.DefaultChallengeScheme = "oidc";
                    })
                    .AddCookie("authCookie")
                    .AddOpenIdConnect("oidc", configureOptions =>
                    {
                        configureOptions.Authority = "https://localhost:44326/";

                        configureOptions.ClientId = "054_mvc";
                        configureOptions.ClientSecret = "sujith_acharya_mvc";

                        configureOptions.SaveTokens = true;

                        configureOptions.ResponseType = "code";

                        configureOptions.GetClaimsFromUserInfoEndpoint = true;

                        configureOptions.SignedOutCallbackPath = "/Home/Index";

                        configureOptions.ClaimActions.MapUniqueJsonKey("raw.pubclaim", "rc.pubclaim");

                        configureOptions.Scope.Add("rc_pub_scope");
                        configureOptions.Scope.Add("ApiOne");
                        configureOptions.Scope.Add("offline_access");
                    });
        }

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
