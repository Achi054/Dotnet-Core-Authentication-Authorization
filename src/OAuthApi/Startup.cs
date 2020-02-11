using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OAuthApi.AuthHandler;
using OAuthApi.AuthRequirement;

namespace OAuthApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication()
                    .AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>("ApiSchema", null);

            services.AddAuthorization(configure =>
            {
                var authBuilder = new AuthorizationPolicyBuilder();
                var authPolicy = authBuilder.AddRequirements(new JwtRequirement()).Build();

                configure.DefaultPolicy = authPolicy;
            });
            services.AddScoped<IAuthorizationHandler, JwtRequirementHandler>();

            services.AddHttpClient().AddHttpContextAccessor();
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
