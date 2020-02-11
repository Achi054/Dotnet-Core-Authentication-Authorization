using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiOne
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options => options.AddPolicy("AllowAll", p =>
                p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            ));

            services.AddAuthentication("Identity")
                    .AddJwtBearer("Identity", configureOptions =>
                    {
                        configureOptions.Authority = "https://localhost:44326/";
                        configureOptions.Audience = "ApiOne";
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
