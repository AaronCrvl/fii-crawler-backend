using NUnit.Framework;
using System;

namespace TestManagement
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RetornoDadosListaFii()
        {
            var lista = new fiiCrawlerApi.WebScraper.Crawler().GetListaResumoFii().Result;

            foreach(var fundo in lista)
                Console.WriteLine($"Item da Lista de Fii: {fundo.nome}");
        }

        [Test]
        public void RetornoDadosFii()
        {
            var fundoDeInvestimento = new fiiCrawlerApi.WebScraper.Crawler().GetInformacaoFII("tepp11").Result;
            Console.WriteLine (
                "Fii Retornado! \t\n" 
                + $"Nome Completo do Fundo: {fundoDeInvestimento.nomeCompleto}\t\n"
                + $"Cotação: {fundoDeInvestimento.cotacaoAtual}\t\n"
                + $"Varicação: {fundoDeInvestimento.variacao}\t\n"
            );
        }
    }
}