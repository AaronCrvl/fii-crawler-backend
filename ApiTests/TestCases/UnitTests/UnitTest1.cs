using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ApiTests.TestCases.UnitTests
{
    public class UnitTest1
    {
        #region Variáveis Privadas
        private fiiCrawlerApi.Models.FIIDetalhado fiiDetalhamentoTeste =
            new fiiCrawlerApi.Models.FIIDetalhado
            {
                codigoFii = "tepp11",
                nomeCompleto = "TELLUS PROPERTIES - FDO INV. IMO",
                variacao = "- 0,10%",
                cota = "R$ 91,31",
            };
        private string pathParaAmbienteDeTeste = @"C:\Users\aaron\source\repos\fiiCrawler_Backend\fiiCrawlerApi\Cache\CacheFiles";
        private string pathListaFii = Path.Combine(Environment.CurrentDirectory, @"StaticData\ListaFII.json");
        private List<fiiCrawlerApi.Models.FII> listaFiiTeste = null;        
        #endregion        

        #region Setup
        [SetUp]
        public void Setup()
        {
            try
            {
                listaFiiTeste = JsonConvert.DeserializeObject<List<fiiCrawlerApi.Models.FII>> (
                    File.ReadAllText (
                        // ajuste para casos de teste
                        this.pathListaFii.Contains("\\bin\\Debug\\net5.0") ?
                        $@"{this.pathParaAmbienteDeTeste}\\FIIDetailCache.json" : this.pathListaFii
                    )
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion        

        #region Testes Unitários - Cache
        [Test]
        public void SalvarCacheLista()
        {
            try
            {
                if (listaFiiTeste != null)
                {
                    var gerenciador = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                    if (gerenciador.ExisteCacheLista())
                    {
                        gerenciador.LimparCacheLista();
                        gerenciador.SalvarCacheLista(listaFiiTeste);
                    }
                    else
                        gerenciador.SalvarCacheLista(listaFiiTeste);

                    Console.Write("Sucesso. Verifique o arquivo de cache e sua data de alteração. ");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Test]
        public void SalvarCacheDetalhamento()
        {
            try
            {
                var gerenciador = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                if (gerenciador.ExisteNoCacheDeDetalhamento(fiiDetalhamentoTeste.codigoFii))
                {
                    gerenciador.LimparCacheDetalhamento();
                    gerenciador.SalvarCacheDetalhamento(fiiDetalhamentoTeste);
                }
                else
                    gerenciador.SalvarCacheDetalhamento(fiiDetalhamentoTeste);

                Console.Write("Sucesso. Verifique o arquivo de cache e sua data de alteração. ");

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RetornaDadosDetalhametoFii()
        {
            try
            {
                var gerenciador = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                if (gerenciador.ExisteNoCacheDeDetalhamento(fiiDetalhamentoTeste.codigoFii))
                {
                    var fundo = gerenciador.RetornarDadosDeCacheDetalhamento(fiiDetalhamentoTeste.codigoFii).Result;
                    Console.WriteLine($"Fundo Detalhado!");
                    Console.WriteLine($"Código: {fundo.codigoFii}");
                    Console.WriteLine($"Nome Completo: {fundo.nomeCompleto}");
                    Console.WriteLine($"Cota: {fundo.cota}");
                    Console.WriteLine($"Código: {fundo.codigoFii}");
                }                                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion        
    }
}