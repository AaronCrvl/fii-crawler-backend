using PuppeteerSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using fiiCrawlerApi.Models;

namespace fiiCrawlerApi.WebScraper
{
    /// <summary>
    /// Classe feita para tratar questões de 
    /// web scraping e web craling da aplicação    
    /// </summary>
    public class Crawler
    {
        #region Variáveis Privadas
        private enum TipoDeDado
        {
            paginaResumo,
            paginaFII,
        }

        private IBrowser browser;
        #endregion

        #region Métodos
        public async Task<FIIDetalhado> GetInformacaoFII(string fii)
        {
            try
            {
                using var browserFetcher = new BrowserFetcher();

                /*
                 * Caminho do browser que vai ser utilizado e configuração headless.
                 * 
                 * Headless browser são navegadores sem interface
                 * que fornecem o controle das páginas da web 
                 * mas são executados através de uma aplicação externa
                */
                
                browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                });

                // Acesso a página base
                var pagina = await browser.NewPageAsync();
                await pagina.GoToAsync($"https://fiis.com.br/resumo/{fii}", 0); // Web Crawler retorna o conjunto de dados na página
                
                return TratarDadosFII(pagina).Result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private async Task<FIIDetalhado> TratarDadosFII(IPage page)
        {
            try
            {
                string jsSeletorNomeFII =
                    @"document.getElementsByClassName('headerTicker__content')[0]?"
                    + @".getElementsByClassName('headerTicker__content__name')[0]?"
                    + @".getElementsByTagName('p')[0]?"
                    + @".innerText;";

                var nomeFii = await page.EvaluateExpressionAsync<string>(jsSeletorNomeFII);

                string jsSeletorCotacao =
                    @"document.getElementsByClassName('quotations-details')[0]?"
                    + @".getElementsByClassName('item quotation')[0]?"
                    + @".getElementsByTagName('div')[0]?"
                    + @".getElementsByClassName('value')[0]?"
                    + @".innerText";
                var cotacao = await page.EvaluateExpressionAsync<string>(jsSeletorCotacao);

                string jsSeletorVariacao =
                    @"document.getElementsByClassName('quotations-details')[0]?"
                    + @".getElementsByClassName('item quotation')[0]"
                    + @".getElementsByTagName('div')[0]?"
                    + @".getElementsByClassName('change')[0]?"
                    + @".getElementsByTagName('span')[0]?"
                    + @".innerText";                                                                        
                var variacao = await page.EvaluateExpressionAsync<string>(jsSeletorVariacao);

                string jsSeletorSinalVariacao =                    
                    @"document.getElementsByClassName('quotations-details')[0]?"
                    + @".getElementsByClassName('item quotation')[0]"
                    + @".getElementsByTagName('div')[0]?"
                    + @".getElementsByClassName('change')[0]?"
                    + @".getElementsByTagName('span')[0]?"
                    + @".getAttribute('class')";
                var sinalVariacao = await page.EvaluateExpressionAsync<string>(jsSeletorSinalVariacao);

                var fii = new FIIDetalhado();
                fii.nomeCompleto = nomeFii;
                fii.cotacaoAtual = cotacao;
                fii.variacao = sinalVariacao.ToLower() == "down" ? $"- {variacao}" : $"+ {variacao}";

                // fechar o browser toda vez que finalizar
                // o processo de scraping & crawling                                      
                await page.CloseAsync();
                await browser.CloseAsync();

                return fii;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<List<FII>> GetListaResumoFii()
        {
            try
            {
                using var browserFetcher = new BrowserFetcher();

                /*
                 * Caminho do browser que vai ser utilizado e configuração headless.
                 * 
                 * Headless browser são navegadores sem interface
                 * que fornecem o controle das páginas da web 
                 * mas são executados através de uma aplicação externa
                */
                browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                });

                // Acesso a página base
                var pagina = await browser.NewPageAsync();
                await pagina.GoToAsync(@"https://fiis.com.br/resumo/", 0); // Web Crawler retorna o conjunto de dados na página

                return TratarDadosListaFii(pagina).Result;                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<FII>> TratarDadosListaFii(IPage page)
        {
            try
            {
                /*
                 * Neste ponto temos o início do scraping da página.
                 * 
                 * O processo é feito para extrair informações específicas da página,
                 * neste contexto os dados de cada célula da tabela de Fii é lido
                 * e utilizado para formar um objeto dentro da aplicação
                 * 
                 * Estrutura base do elemento HTML
                 * <tr>
	                    (td[0]) <td>Nome do Fundo</td>
                        (td[1]) <td>Último Rendimento (R$)</td>	                    
                        (td[2]) <td>Último Rendimento</td>	                   
                        (td[3]) <td>Data Pagamento</td>	                   	                    
                        (td[4]) <td>Data Base</td>	                   	                    
                        (td[5]) <td>Rend. Méd. 12m (R$)</td>	                   
                        (td[6]) <td>Patrimônio/Cota</td>
                        (td[7]) <td>Cota base</td>	                    
                   </tr>
                 */
                string jsSelecionarTodosOsNomes = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[0]?.innerText);";
                var nomesFii = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsNomes);

                string jsSelecionarTodosOsUltimosRendimentosRs = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[1]?.innerText);";
                var ultimosRendimentosRs = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsUltimosRendimentosRs);

                string jsSelecionarTodosOsUltimosRendimentos = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[2]?.innerText);";
                var ultimosRendimentos = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsUltimosRendimentos);

                string jsSelecionarTodosAsDatasDePagamento = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[3]?.innerText);";
                var datasDePagamento = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosAsDatasDePagamento);

                string jsSelecionarTodosAsDatasBase = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[4]?.innerText);";
                var datasBase = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosAsDatasBase);

                string jsSelecionarTodosOsRedimentoMedioAnual = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[5]?.innerText);";
                var rendimentoMedioAnual = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsRedimentoMedioAnual);

                string jsSelecionarTodosOsPatrimonios = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[6]?.innerText);";
                var patrimonios = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsPatrimonios);

                string jsSelecionarTodasAsCotas = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[7]?.innerText);";
                var cotas = await page.EvaluateExpressionAsync<string[]>(jsSelecionarTodasAsCotas);

                // Criando novos objetos DTO com base nos dados recebidos.
                List<FII> fiis = new List<FII>();
                if (nomesFii.Length > 0)
                {
                    // a primeira linha é o nome das colunas da tabela,
                    // como este dado não será utilizado o mesmo pode ser ignorado
                    for (int i = 1; i < ultimosRendimentos.Length; ++i)
                        fiis.Add(
                            new FII
                            {
                                nome = nomesFii[i],
                                ultimoRedimentoRS = ultimosRendimentosRs[i],
                                ultimosRedimento = ultimosRendimentos[i],
                                dataPagamento = datasDePagamento[i],
                                dataBase = datasBase[i],
                                rendimentoMedioAnual = rendimentoMedioAnual[i],
                                patrimonio = patrimonios[i],
                                cota = cotas[i]
                            }
                        );
                }

                // fechar o browser toda vez que finalizar
                // o processo de scraping & crawling
                await page.CloseAsync();                
                await browser.CloseAsync();
                return fiis;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        #endregion        
    }
}