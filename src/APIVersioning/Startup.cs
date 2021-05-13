using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace APIVersioning
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // from Microsoft.AspNetCore.Mvc.Versioning
            //services.AddApiVersioning(); // ?api-version=X.X needs to be provided as part of the request

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; // 1.0

                options.DefaultApiVersion = new ApiVersion(1, 0); // Setting it as 1.0 is the same as ApiVersion.Default

                //options.ApiVersionReader = new HeaderApiVersionReader("APIVersion"); // overrides the default queryString behavior - ?api-version=X.X

                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("APIVersion"),
                    new QueryStringApiVersionReader("APIVersion")
                ); // Combine different options, such as headers and query string

                options.ReportApiVersions = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}