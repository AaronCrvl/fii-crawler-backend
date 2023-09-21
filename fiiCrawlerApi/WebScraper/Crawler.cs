using PuppeteerSharp;
using AngleSharp;
using AngleSharp.Dom;
using System.Threading.Tasks;
using System.Collections.Generic;
using fiiCrawlerApi.Models;

namespace fiiCrawlerApi.WebScraper
{
    public class Crawler
    {
        public async Task<List<Fii>> GetListaResumoFii()
        {
            try
            {
                using var browserFetcher = new BrowserFetcher();
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                });

                var pagina = await browser.NewPageAsync();
                await pagina.GoToAsync(@"https://fiis.com.br/resumo/");

                return await TratarDadosRecebidos(pagina);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<Fii>> TratarDadosRecebidos(IPage page)
        {
            try
            {                
                /*
                    tr {
	                    td[0] = <a href=Link para a página do Fii>
	                    td[1] = Último Rendimento (R$)
	                    td[2] = Último Rendimento
	                    td[3] = Data Pagamento
	                    td[4] = Data Base
	                    td[5] = Rend. Méd. 12m (R$)
	                    td[6] = Patrimônio/Cota
	                    td[7] = Cota base
                    }             
                */   

                // Separando informações do Fii por coluna                                                                                                                
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
                List<Fii> fiis = new List<Fii>();
                if (nomesFii.Length > 0)
                {
                    for (var i = 0; i < ultimosRendimentos.Length; ++i)
                        fiis.Add(
                            new Fii { 
                                fii = nomesFii[i],
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

                return fiis;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}