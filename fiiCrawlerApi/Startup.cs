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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "fiiCrawlerApi", Version = "v1" });
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
            startSchedulerAsync();
        }

        /*
         * Tarefas para verificar e atualizar automaticamente
         * o cache utilizado pela aplicação.
         * 
         * O intuito desta funcionadlidade é otimizar o
         * processo de acesso aos dados da api visto que a mesma
         * utilizar um webcrawler para compor os dados.
         */    
        private static async Task startSchedulerAsync()
        {
            // ref: https://www.quartz-scheduler.net/documentation/

            IJobDetail job = JobBuilder.Create<MonitorarCacheJob>()
            .WithIdentity(name: "BackgroundJob", group: "JobGroup")
            .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(name: "RepeatingTrigger", group: "TriggerGroup")
            .WithSimpleSchedule(o => o
                .RepeatForever()
                .WithIntervalInMinutes(30))
            .Build();

            IHost builder = Host.CreateDefaultBuilder()
                .ConfigureServices((cxt, services) =>
                {
                    services.AddQuartz();
                    services.AddQuartzHostedService(opt =>
                    {
                        opt.WaitForJobsToComplete = true;
                    });
                }).Build();

            var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            await scheduler.ScheduleJob(job, trigger);
            await builder.RunAsync();
        }
    }
}
