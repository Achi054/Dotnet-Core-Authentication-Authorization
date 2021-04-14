using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace WeatherClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthorization(config =>
            {
                config.AddPolicy("CanAccess", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireClaim("country", "nl");
                    policyBuilder.RequireClaim("subscriptionlevel", "premium");
                });
            });

            services.AddAuthentication(opts =>
                {
                    opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
                {
                    opts.AccessDeniedPath = "/Authorize/AccessDenied";
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config =>
                {
                    config.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    config.Authority = "https://localhost:44336/";
                    config.ClientId = "0D0B79C4-736A-478A-8249-1CEB8A56F818";
                    config.ClientSecret = "clientsecret";
                    config.ResponseType = "code";
                    config.Scope.Add("address");
                    config.Scope.Add("roles");
                    config.Scope.Add("weatherapi");
                    config.Scope.Add("country");
                    config.Scope.Add("subscriptionlevel");
                    config.Scope.Add("offline_access");
                    config.SaveTokens = true;
                    config.GetClaimsFromUserInfoEndpoint = true;
                    config.ClaimActions.DeleteClaims("sid", "idp", "auth_time", "s_hash");
                    config.ClaimActions.MapUniqueJsonKey("role", "role");
                    config.ClaimActions.MapUniqueJsonKey("country", "country");
                    config.ClaimActions.MapUniqueJsonKey("subscriptionlevel", "subscriptionlevel");
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.GivenName,
                        RoleClaimType = JwtClaimTypes.Role,
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
