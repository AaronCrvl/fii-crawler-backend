using fiiCrawlerApi.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fiiCrawlerApi.WebScraper
{
    /// <summary>
    /// Classe feita para tratar questões de 
    /// Web Scraping da aplicação.
    /// 
    /// Com o Web Scraping, obtemos informações específicas
    /// dentro da página.
    /// </summary>
    public class Scraper
    {
        #region Variaveis Privadas
        private IBrowser navegador;
        #endregion

        #region Construtor
        public Scraper(ref IBrowser _navegador)
        {
            navegador = _navegador;
        }
        #endregion

        #region Métodos
        public async Task<FIIDetalhado> CrawlDadosFII(string codigoFii, IPage paginaWeb)
        {
            /*
           * Neste ponto temos o início do scraping da página.
           * 
           * O processo é feito para extrair informações específicas da página,
           * neste contexto os dados de cada célula da tabela de Fii é lido
           * e utilizado para formar um objeto dentro da aplicação
           * 
           * Estrutura base do elemento HTML            
           */

            try
            {
                string jsSeletorNomeFII =
                    @"document.getElementsByClassName('headerTicker__content')[0]?"
                    + @".getElementsByClassName('headerTicker__content__name')[0]?"
                    + @".getElementsByTagName('p')[0]?"
                    + @".innerText;";

                var nomeFii = await paginaWeb.EvaluateExpressionAsync<string>(jsSeletorNomeFII);

                string jsSeletorCotacao =
                    @"document.getElementsByClassName('quotations-details')[0]?"
                    + @".getElementsByClassName('item quotation')[0]?"
                    + @".getElementsByTagName('div')[0]?"
                    + @".getElementsByClassName('value')[0]?"
                    + @".innerText";
                var cotacao = await paginaWeb.EvaluateExpressionAsync<string>(jsSeletorCotacao);

                string jsSeletorVariacao =
                    @"document.getElementsByClassName('quotations-details')[0]?"
                    + @".getElementsByClassName('item quotation')[0]"
                    + @".getElementsByTagName('div')[0]?"
                    + @".getElementsByClassName('change')[0]?"
                    + @".getElementsByTagName('span')[0]?"
                    + @".innerText";
                var variacao = await paginaWeb.EvaluateExpressionAsync<string>(jsSeletorVariacao);

                string jsSeletorSinalVariacao =
                    @"document.getElementsByClassName('quotations-details')[0]?"
                    + @".getElementsByClassName('item quotation')[0]"
                    + @".getElementsByTagName('div')[0]?"
                    + @".getElementsByClassName('change')[0]?"
                    + @".getElementsByTagName('span')[0]?"
                    + @".getAttribute('class')";
                var sinalVariacao = await paginaWeb.EvaluateExpressionAsync<string>(jsSeletorSinalVariacao);

                var fii = new FIIDetalhado();
                fii.codigoFii = codigoFii;
                fii.nomeCompleto = nomeFii;
                fii.cota = cotacao;
                fii.variacao = sinalVariacao.ToLower() == "down" ? $"-{variacao}" : $"+{variacao}";

                // fechar o browser toda vez que finalizar
                // o processo de scraping & crawling                                      
                await paginaWeb.CloseAsync();
                await this.navegador.CloseAsync();
                return fii;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<List<FII>> CrawlDadosListaFii(IPage paginaWeb)
        {
            /*
            * Neste ponto temos o início do scraping da página.
            * 
            * O processo é feito para extrair informações específicas da página,
            * neste contexto os dados de cada célula da tabela de Fii é lido
            * e utilizado para formar um objeto dentro da aplicação
            * 
            * Estrutura base do elemento HTML            
            */          
            try
            {
                string jsSelecionarTodosOsNomes = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[0]?.innerText);";
                var nomesFii = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsNomes);

                string jsSelecionarTodosOsUltimosRendimentosRs = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[1]?.innerText);";
                var ultimosRendimentosRs = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsUltimosRendimentosRs);

                string jsSelecionarTodosOsUltimosRendimentos = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[2]?.innerText);";
                var ultimosRendimentos = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsUltimosRendimentos);

                string jsSelecionarTodosAsDatasDePagamento = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[3]?.innerText);";
                var datasDePagamento = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosAsDatasDePagamento);

                string jsSelecionarTodosAsDatasBase = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[4]?.innerText);";
                var datasBase = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosAsDatasBase);

                string jsSelecionarTodosOsRedimentoMedioAnual = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[5]?.innerText);";
                var rendimentoMedioAnual = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsRedimentoMedioAnual);

                string jsSelecionarTodosOsPatrimonios = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[6]?.innerText);";
                var patrimonios = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodosOsPatrimonios);

                string jsSelecionarTodasAsCotas = @"Array.from(document.querySelectorAll('tr')).map(tr => tr.getElementsByTagName('td')[7]?.innerText);";
                var cotas = await paginaWeb.EvaluateExpressionAsync<string[]>(jsSelecionarTodasAsCotas);

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
                await paginaWeb.CloseAsync();
                await this.navegador.CloseAsync();
                return fiis;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
    #endregion
}
