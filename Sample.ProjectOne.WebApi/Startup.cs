using DesertCamel.BaseMicroservices.SuperBootstrap.Base;
using DesertCamel.BaseMicroservices.SuperBootstrap.Cache;

namespace Sample.ProjectOne.WebApi
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
            // other services
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddBootstrapBase(Configuration);
            services.AddBootstrapCache(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // initial db migration/setup if any

            app.UseRouting();
            app.UseCors();  // remember to always implement this
            app.UseAuthorization();

            app.UseBootstrapBase(); // set this exactly before app.UseEndpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
