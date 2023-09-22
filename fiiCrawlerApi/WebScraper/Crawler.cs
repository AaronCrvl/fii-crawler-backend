using PuppeteerSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using fiiCrawlerApi.Models;

namespace fiiCrawlerApi.WebScraper
{
    /// <summary>
    /// Classe feita para tratar questões de 
    /// Web Crawling da aplicação.
    /// 
    /// Com o Web Crawling, obtemos informações específicas
    /// gerais da página.
    /// </summary>
    public class Crawler
    {
        #region Variáveis Privadas
        private IBrowser navegador;
        private Scraper scraper;
        private string caminhoArquivoExecutavelNavegador = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
        #endregion

        #region Construtor     
        #endregion        

        #region Métodos
        public async Task<List<FII>> ScrapeListaResumoFii()
        {
            try
            {
                /*
                 * Caminho do browser que vai ser utilizado e configuração headless.
                 * 
                 * Headless browser são navegadores sem interface
                 * que fornecem o controle das páginas da web 
                 * mas são executados através de uma aplicação externa
                */
                this.navegador = await Puppeteer.LaunchAsync(
                    new LaunchOptions
                    {
                        Headless = true,
                        ExecutablePath = this.caminhoArquivoExecutavelNavegador
                    });

                // passando a referência da instância do navegador para que
                // o mesmo possa ser fechado após a extração de dados
                this.scraper = new Scraper(ref navegador);

                // Acesso a página base
                var pagina = await navegador.NewPageAsync();
                await pagina.GoToAsync(@"https://fiis.com.br/resumo/", 0); // Web Crawler retorna o conjunto de dados na página
                return this.scraper.CrawlDadosListaFii(pagina).Result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FIIDetalhado> ScrapeInformacaoFII(string codigoFii)
        {
            try
            {                
                /*
                 * Caminho do browser que vai ser utilizado e configuração headless.
                 * 
                 * Headless browser são navegadores sem interface
                 * que fornecem o controle das páginas da web 
                 * mas são executados através de uma aplicação externa
                */
                
                this.navegador = await Puppeteer.LaunchAsync(
                    new LaunchOptions
                    {
                        Headless = true,
                        ExecutablePath = this.caminhoArquivoExecutavelNavegador
                    });

                // passando a referência da instância do navegador para que
                // o mesmo possa ser fechado após a extração de dados
                scraper = new Scraper(ref navegador);

                // Acesso a página base
                var paginaWeb = await navegador.NewPageAsync();
                await paginaWeb.GoToAsync($"https://fiis.com.br/resumo/{codigoFii}", 0); // Web Crawler retorna o conjunto de dados na página                
                return scraper.CrawlDadosFII(codigoFii, paginaWeb).Result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }                
        #endregion        
    }
}