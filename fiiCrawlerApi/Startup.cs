using fiiCrawlerApi.QuartzScheduler.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Threading.Tasks;

namespace fiiCrawlerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }    
        private fiiCrawlerApi.QuartzScheduler.Scheduler scheduler = new fiiCrawlerApi.QuartzScheduler.Scheduler();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new OpenApiInfo { 
                        Title = "fiiCrawlerApi", 
                        Version = "v1",
                        Contact = new OpenApiContact { 
                                Name = "Aaron Carvalho", 
                                Url = new System.Uri("https://github.com/AaronCrvl"), 
                                Email = "carvalhosins@gmail.com" 
                        } 
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "fiiCrawlerApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Chamada sem await para 
            // a função de comporte de forma assíncrona
            this.scheduler.startSchedulerAsync();
        }

            
    }
}
