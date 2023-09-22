using fiiCrawlerApi.QuartzScheduler.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fiiCrawlerApi.QuartzScheduler
{
    public class Scheduler
    {
        /*
         * Tarefas para verificar e atualizar automaticamente
         * o cache utilizado pela aplicação.
         * 
         * O intuito desta funcionadlidade é otimizar o
         * processo de acesso aos dados da api visto que a mesma
         * utilizar um webcrawler para compor os dados.
         */
        public async Task startSchedulerAsync()
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
