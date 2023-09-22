using NUnit.Framework;
using System;

namespace ApiTests.TestCases.IntegrationTests
{
    public class IntegrationTest1
    {
        #region Testes de Integração - Módulo WebScraper x Módulo Gerenciamento de Cache        
        [Test]
        public void RetornoDadosListaFii()
        {
            try
            {
                var lista = new fiiCrawlerApi.WebScraper.Crawler().ScrapeListaResumoFii().Result;
                foreach (var fundo in lista)
                    Console.WriteLine($"Item da Lista de Fii: {fundo.nome}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Test]
        public void RetornoDadosFii()
        {
            try
            {
                var fundoDeInvestimento = new fiiCrawlerApi.WebScraper.Crawler().ScrapeInformacaoFII("tepp11").Result;
                Console.WriteLine(
                    "Fii Retornado! \t\n"
                    + $"Código: {fundoDeInvestimento.codigoFii}\t\n"
                    + $"Nome Completo: {fundoDeInvestimento.nomeCompleto}\t\n"
                    + $"Cotação: {fundoDeInvestimento.cota}\t\n"
                    + $"Varicação: {fundoDeInvestimento.variacao}\t\n"
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
