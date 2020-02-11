using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OAuthServer.Constants;

namespace OAuthServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth")
                    .AddJwtBearer("OAuth", options =>
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (context.Request.Query.ContainsKey("access_token"))
                                    context.Token = context.Request.Query["access_token"];

                                return Task.CompletedTask;
                            }
                        };

                        var securitySignature = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConstants.SecretKey));
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = securitySignature,
                            ValidAudience = TokenConstants.Audience,
                            ValidIssuer = TokenConstants.Issuer
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
