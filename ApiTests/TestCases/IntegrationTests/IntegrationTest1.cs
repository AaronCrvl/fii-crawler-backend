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
                var lista = new fiiCrawlerApi.WebScraper.Crawler().CrawlListaResumoFii().Result;
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
                var fundoDeInvestimento = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFII("tepp11").Result;
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

        [Test]
        public void RetornoDadosFii_Dividendos()
        {
            try
            {
                var fundoDeInvestimento = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFII("bcff11").Result;
                Console.WriteLine(
                    "Dividendos do Fii Retornado! \t\n"
                    + $"Código: {fundoDeInvestimento.codigoFii}\t\n"
                    + $"Cotação: {fundoDeInvestimento.cota}\t\n"
                    + $"Varicação: {fundoDeInvestimento.variacao}\t\n"
                );

                Console.WriteLine($"Dividendos Retornados: ");
                foreach (var dividendo in fundoDeInvestimento.historicoDividendos)
                    Console.WriteLine(
                        $"Cotação Base: {dividendo.cotacaoBase}\t\n"
                        + $"Data Base: {dividendo.dataBase}\t\n"
                        + $"Data de Pagamento: {dividendo.dataPagamento}\t\n"
                        + $"Rendimento: {dividendo.rendimento}\t\n"
                    );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Test]
        public void RetornoDadosFii_Administrador()
        {
            try
            {
                var fundoDeInvestimento = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFII("hgff11").Result;
                Console.WriteLine(
                    $"Dividendos do Administrador do {fundoDeInvestimento.codigoFii.ToUpper()}! \t\n"
                    + $"CNPJ: {fundoDeInvestimento.administrador.cnpj}\t\n"
                    + $"Email: {fundoDeInvestimento.administrador.email}\t\n"
                    + $"Nome Pregão: {fundoDeInvestimento.administrador.nomeNoPregao}\t\n"
                    + $"Patrimônio: {fundoDeInvestimento.administrador.patrimonio}\t\n"
                    + $"Razão Social: {fundoDeInvestimento.administrador.razaoSocial}\t\n"
                    + $"Público Alvo: {fundoDeInvestimento.administrador.publicoAlvo}\t\n"
                    + $"Segmento: {fundoDeInvestimento.administrador.segmento}\t\n"
                    + $"Número de Cotas: {fundoDeInvestimento.administrador.numeroDeCotas}\t\n"
                    + $"Site: {fundoDeInvestimento.administrador.site}\t\n"
                );

                Console.WriteLine();
                Console.WriteLine();

                var fundoDeInvestimento2 = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFII("bcff11").Result;
                Console.WriteLine(
                    $"Dividendos do Administrador do {fundoDeInvestimento2.codigoFii.ToUpper()}! \t\n"
                    + $"CNPJ: {fundoDeInvestimento2.administrador.cnpj}\t\n"
                    + $"Email: {fundoDeInvestimento2.administrador.email}\t\n"
                    + $"Nome Pregão: {fundoDeInvestimento2.administrador.nomeNoPregao}\t\n"
                    + $"Patrimônio: {fundoDeInvestimento2.administrador.patrimonio}\t\n"
                    + $"Razão Social: {fundoDeInvestimento2.administrador.razaoSocial}\t\n"
                    + $"Público Alvo: {fundoDeInvestimento2.administrador.publicoAlvo}\t\n"
                    + $"Segmento: {fundoDeInvestimento2.administrador.segmento}\t\n"
                    + $"Número de Cotas: {fundoDeInvestimento2.administrador.numeroDeCotas}\t\n"
                    + $"Site: {fundoDeInvestimento2.administrador.site}\t\n"
                );


                Console.WriteLine();
                Console.WriteLine();

                var fundoDeInvestimento3 = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFII("tepp11").Result;
                Console.WriteLine(
                    $"Dividendos do Administrador do {fundoDeInvestimento3.codigoFii.ToUpper()}! \t\n"
                    + $"CNPJ: {fundoDeInvestimento3.administrador.cnpj}\t\n"
                    + $"Email: {fundoDeInvestimento3.administrador.email}\t\n"
                    + $"Nome Pregão: {fundoDeInvestimento3.administrador.nomeNoPregao}\t\n"
                    + $"Patrimônio: {fundoDeInvestimento3.administrador.patrimonio}\t\n"
                    + $"Razão Social: {fundoDeInvestimento3.administrador.razaoSocial}\t\n"
                    + $"Público Alvo: {fundoDeInvestimento3.administrador.publicoAlvo}\t\n"
                    + $"Segmento: {fundoDeInvestimento3.administrador.segmento}\t\n"
                    + $"Número de Cotas: {fundoDeInvestimento3.administrador.numeroDeCotas}\t\n"
                    + $"Site: {fundoDeInvestimento3.administrador.site}\t\n"
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
