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
                    cache.ExisteCacheLista()
                    && DateTime.Now.Year == cache.ultimaModificacaoLista.Year
                    && DateTime.Now.Month == cache.ultimaModificacaoLista.Month
                    && DateTime.Now.Day == cache.ultimaModificacaoLista.Day                    
                    && DateTime.Now.Hour == cache.ultimaModificacaoLista.Hour
                    && (DateTime.Now.Minute - cache.ultimaModificacaoLista.Minute) <= 30 // atualizar a cada 30 minutos
                )
                await Task.FromResult(true);
            else
            {
                cache.LimparCacheLista();
                cache.SalvarCacheLista(new fiiCrawlerApi.WebScraper.Crawler().ScrapeListaResumoFii().Result);
            }

            await Task.FromResult(true);
        }
    }
}
