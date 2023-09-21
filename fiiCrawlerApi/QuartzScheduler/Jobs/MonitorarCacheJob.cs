using Quartz;
using System;
using System.Threading.Tasks;

//ref: https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/quartz-jobs.html
namespace fiiCrawlerApi.QuartzScheduler.Jobs
{
    /// <summary>
    /// Classe feita para tratar questões de 
    /// gerencimaneto de cache da aplicação
    /// </summary>
    public class MonitorarCacheJob : Quartz.IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var cache = new fiiCrawlerApi.Cache.GerenciadorDeCache();
            if (
                    cache.ExisteCache()
                    && DateTime.Now.Year == cache.ultimaModificacao.Year
                    && DateTime.Now.Month == cache.ultimaModificacao.Month
                    && DateTime.Now.Day == cache.ultimaModificacao.Day                    
                    && DateTime.Now.Hour == cache.ultimaModificacao.Hour
                    && (DateTime.Now.Minute - cache.ultimaModificacao.Minute) <= 30 // atualizar a cada 30 minutos
                )
                await Task.FromResult(true);
            else
            {
                cache.LimparCache();
                cache.SalvarCache(new fiiCrawlerApi.WebScraper.Crawler().GetListaResumoFii().Result);
            }

            await Task.FromResult(true);
        }
    }
}
