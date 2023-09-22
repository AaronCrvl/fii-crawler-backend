using fiiCrawlerApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Cache
{
    public class GerenciadorDeCache
    {
        #region Variáveis Privadas 
        private string _caminhoListaFii = Path.Combine(Environment.CurrentDirectory, @"Cache\CacheFiles\FIICacheList.json");
        private DateTime _ultimaModificacaoLista = File.GetLastWriteTime(Path.Combine(Environment.CurrentDirectory, @"Cache\CacheFiles\FIICacheList.json"));
        private string _caminhoDetalhamentoFii = Path.Combine(Environment.CurrentDirectory, @"Cache\CacheFiles\FIIDetailCache.json");
        private DateTime _ultimaModificacaoDetalhamento = File.GetLastWriteTime(Path.Combine(Environment.CurrentDirectory, @"Cache\CacheFiles\FIIDetailCache.json"));
        private string pathParaAmbienteDeTeste = @"C:\Users\aaron\source\repos\fiiCrawler_Backend\fiiCrawlerApi\Cache\CacheFiles";
        #endregion

        #region Propriedades
        public string caminhoListaFii
        {
            get { return this._caminhoListaFii; }
            set { this._caminhoListaFii = value; }
        }

        public string caminhoDetalhamentoFii
        {
            get { return this._caminhoDetalhamentoFii; }
            set { this._caminhoDetalhamentoFii = value; }
        }

        public DateTime ultimaModificacaoLista
        {
            get { return this._ultimaModificacaoLista; }
            set { this._ultimaModificacaoLista = value; }
        }
        public DateTime ultimaModificacaoDetalhamento
        {
            get { return this._ultimaModificacaoDetalhamento; }
            set { this._ultimaModificacaoDetalhamento = value; }
        }
        #endregion

        #region Construtores
        public GerenciadorDeCache() { }
        #endregion

        #region Métodos
        public bool ExisteCacheLista()
        {
            var res = false;
            if (File.Exists(this.caminhoListaFii) && File.ReadAllText(this.caminhoListaFii).Length > 0)
                res = true;

            return res;
        }

        public bool ExisteNoCacheDeDetalhamento(string codigoFii)
        {
            // validar se o FII existe dentro da lista do cache
            var cache = 
                System.IO.File.ReadAllText(
                    // ajuste para casos de teste
                    this.caminhoDetalhamentoFii.Contains("\\bin\\Debug\\net5.0") ?
                    $@"{this.pathParaAmbienteDeTeste}\\FIIDetailCache.json" : this.caminhoDetalhamentoFii
                );
            if (cache.Length == 0)
                return false;

            var lista = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cache);            
            foreach (var fii in lista)
                if (fii.codigoFii.ToLower() == codigoFii.ToLower())
                    return true;
           
            return false;
        }


        public void LimparCacheLista()
        {
            try
            {
                if (ExisteCacheLista())
                    System.IO.File.WriteAllText(this.caminhoListaFii, string.Empty); //override cache text
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void LimparCacheDetalhamento()
        {
            try
            {                
                System.IO.File.WriteAllText(this.caminhoDetalhamentoFii, string.Empty); //override cache text
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SalvarCacheLista(List<FII> list)
        {
            try
            {
                var json = JsonConvert.SerializeObject(list);
                System.IO.File.WriteAllText(this.caminhoListaFii, json); //write string to file
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SalvarCacheDetalhamento(FIIDetalhado fii)
        {
            try
            {                
                var dadosArquivo = File.ReadAllText (
                        // ajuste para casos de teste
                        this.caminhoDetalhamentoFii.Contains("\\bin\\Debug\\net5.0") ?
                        $@"{this.pathParaAmbienteDeTeste}\\FIIDetailCache.json" : this.caminhoDetalhamentoFii
                    );
                var listaDadosDetalhamento = JsonConvert.DeserializeObject<List<FIIDetalhado>>(dadosArquivo);
                if (listaDadosDetalhamento == null)
                    listaDadosDetalhamento = new List<FIIDetalhado>();

                listaDadosDetalhamento.Add(fii);

                var json = JsonConvert.SerializeObject(listaDadosDetalhamento);
                System.IO.File.WriteAllText(this.caminhoDetalhamentoFii, json); //write string to file
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<FII>> RetornarDadosDeCacheLista()
        {
            try
            {
                var cache = System.IO.File.ReadAllText(this.caminhoListaFii);
                var lista = JsonConvert.DeserializeObject<List<FII>>(cache);
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<FIIDetalhado> RetornarDadosDeCacheDetalhamento(string fiiBusca)
        {
            try
            {
                var cacheDetalhamento = System.IO.File.ReadAllText(this.caminhoDetalhamentoFii);
                var listaDetalhamento = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cacheDetalhamento);
                FIIDetalhado fiiDesejado = new FIIDetalhado { codigoFii = "", nomeCompleto = "", cota = "", variacao = "", valorizacao = "", };

                // buscar FII dentro do cache
                if (listaDetalhamento.Count > 0)
                    foreach (var fii in listaDetalhamento)
                        if (fii.codigoFii == fiiBusca)
                        {
                            fiiDesejado = new FIIDetalhado
                            {
                                codigoFii = fii.codigoFii,
                                nomeCompleto = fii.nomeCompleto,
                                cota = fii.cota,
                                variacao = fii.variacao,
                                valorizacao = fii.valorizacao,
                            };
                        }

                // acionar o web crawler para buscar informação do fii
                if (fiiDesejado.codigoFii == "") 
                {
                    // validar se o FII existe dentro da lista do cache
                    var cacheLista = System.IO.File.ReadAllText(this.caminhoDetalhamentoFii);
                    var lista = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cacheDetalhamento);
                    bool encontrouFiiNaLista = false;

                    foreach (var fii in lista)
                        if (fii.codigoFii.ToLower() == fiiBusca.ToLower())
                            encontrouFiiNaLista = true;

                    if (encontrouFiiNaLista)
                    {
                        var crawler = new fiiCrawlerApi.WebScraper.Crawler();
                        fiiDesejado = await crawler.ScrapeInformacaoFII(fiiBusca);
                        return fiiDesejado;
                    }
                    else
                        return fiiDesejado;
                }
                // retornar FII encontrado no cache
                else
                    return fiiDesejado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion        
    }
}