using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services
                .AddIdentity<IdentityUser, IdentityRole>(option =>
                {
                    option.Password.RequireDigit = true;
                    option.Password.RequiredLength = 5;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(configure =>
            {
                configure.Cookie.Name = "Identity.Cookie";
                configure.LoginPath = "/Home/Login";
                configure.LogoutPath = "/Home/Logout";
            });

            services.AddIdentityServer()
                    .AddAspNetIdentity<IdentityUser>()
                    //.AddInMemoryApiResources(Configuration.GetApiResources())
                    //.AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                    //.AddInMemoryClients(Configuration.GetClients())
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                    // this adds the operational data from DB (codes, tokens, consents)
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));

                        // this enables automatic token cleanup. this is optional.
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    })
                    .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
