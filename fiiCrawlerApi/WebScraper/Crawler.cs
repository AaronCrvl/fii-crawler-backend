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
        /// <summary>
        /// Web Crawling da lista de FII's        
        /// </summary>
        public async Task<List<FII>> CrawlListaResumoFii()
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
                System.Console.WriteLine("Crawled!");

                return this.scraper.ScrapeDadosListaFii(pagina).Result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                // garantir que a instância do navegador criada seja fechada
                // Esse processo providencia um ciclo de vida melhor para a aplicação
                // e uso de memória no ambiente em que a api for publicada
                if (this.navegador != null && !this.navegador.IsClosed)
                    await this.navegador.CloseAsync();                
            }
        }

        /// <summary>
        /// Web Crawling da lista de detalhamento dos FII's        
        /// </summary>
        public async Task<FIIDetalhado> CrawlInformacaoFII(string codigoFii)
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
                System.Console.WriteLine("Crawled!");

                return scraper.ScrapelDadosFII(codigoFii, paginaWeb).Result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Garantir que a instância do navegador criada seja fechada.
                // Esse processo providencia um ciclo de vida melhor para a aplicação
                // e uso de memória no ambiente em que a api for publicada
                if (this.navegador != null && !this.navegador.IsClosed)
                    await this.navegador.CloseAsync();                
            }
        }
        #endregion
    }
}