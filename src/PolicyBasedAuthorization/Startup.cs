using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicyBasedAuthorization.Authorization;

namespace PolicyBasedAuthorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthentication("UserAuth")
                .AddCookie("UserAuth", configOption =>
                {
                    configOption.Cookie.Name = "UserAuth";
                    configOption.LoginPath = "/Home/Login";
                });

            //services.AddAuthorization(configure =>
            //{
            //    var policy = new AuthorizationPolicyBuilder();
            //    policy.RequireAuthenticatedUser();
            //    policy.RequireClaim(ClaimTypes.DateOfBirth);

            //    configure.DefaultPolicy = policy.Build();

            //    configure.AddPolicy("Claim.Dob", configure.DefaultPolicy);
            //});

            services.AddAuthorization(configure =>
            {
                configure.AddPolicy("Claim.Dob", policy => policy.AddRequirements(new CustomClaim(ClaimTypes.DateOfBirth)));
            });

            services.AddAuthorization(configure =>
            {
                configure.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

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
