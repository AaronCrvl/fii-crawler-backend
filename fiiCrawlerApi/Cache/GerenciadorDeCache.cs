using fiiCrawlerApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Cache
{
    /// <summary>
    /// Classe responsável por gerenciar o cache
    /// da aplicação.    
    /// </summary>
    public class GerenciadorDeCache
    {
        #region Variáveis Privadas 
        private string _caminhoListaFii =
            Path.Combine(Environment.CurrentDirectory.Contains("\\bin\\Debug\\net5.0") ?
                    Environment.CurrentDirectory.Replace("\\bin\\Debug\\net5.0", "") : Environment.CurrentDirectory
            , @"Cache\CacheFiles\FIICacheList.json");

        private DateTime _ultimaModificacaoLista = File.GetLastWriteTime(Path.Combine(Environment.CurrentDirectory.Contains("\\bin\\Debug\\net5.0") ?
                    Environment.CurrentDirectory.Replace("\\bin\\Debug\\net5.0", "") : Environment.CurrentDirectory
            , @"Cache\CacheFiles\FIICacheList.json"));

        private string _caminhoDetalhamentoFii = Path.Combine(Environment.CurrentDirectory.Contains("\\bin\\Debug\\net5.0") ?
                    Environment.CurrentDirectory.Replace("\\bin\\Debug\\net5.0", "") : Environment.CurrentDirectory
            , @"Cache\CacheFiles\FIIDetailCache.json");

        private DateTime _ultimaModificacaoDetalhamento = File.GetLastWriteTime(Path.Combine(Environment.CurrentDirectory.Contains("\\bin\\Debug\\net5.0") ?
                    Environment.CurrentDirectory.Replace("\\bin\\Debug\\net5.0", "") : Environment.CurrentDirectory
            , @"Cache\CacheFiles\FIIDetailCache.json"));
        #endregion

        #region Propriedades
        public string caminhoListaFii
        {
            get { return this._caminhoListaFii; }
        }

        public string caminhoDetalhamentoFii
        {
            get { return this._caminhoDetalhamentoFii; }
        }

        public DateTime ultimaModificacaoLista
        {
            get { return this._ultimaModificacaoLista; }
        }
        public DateTime ultimaModificacaoDetalhamento
        {
            get { return this._ultimaModificacaoDetalhamento; }
        }
        #endregion

        #region Construtores
        public GerenciadorDeCache() { }
        #endregion

        #region Métodos
        /// <summary>
        /// Verifica a existência de cache para a 
        /// lista de resumo de FII's
        /// </summary>
        public bool ExisteCacheLista()
        {
            var res = false;
            if (File.Exists(this.caminhoListaFii) && File.ReadAllText(this.caminhoListaFii).Length > 0)
                res = true;

            return res;
        }

        /// <summary>
        /// Verifica a existência de cache para algum
        /// FII's espefíco
        /// </summary>
        public bool ExisteNoCacheDeDetalhamento(string codigoFii, string idUser)
        {
            if (idUser is null)
            {
                throw new ArgumentNullException(nameof(idUser));
            }
            // validar se o FII existe dentro da lista do cache
            var cache = System.IO.File.ReadAllText(this.caminhoDetalhamentoFii);
            if (cache.Length == 0)
                return false;

            var lista = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cache);
            foreach (var fii in lista)
                if (fii.codigoFii.ToLower() == codigoFii.ToLower()
                    && fii.userId == idUser)
                    return true;

            return false;
        }

        /// <summary>
        /// Limpar cache da lista de resumo        
        /// </summary>
        public void LimparCacheLista()
        {
            try
            {
                if (ExisteCacheLista())
                    File.WriteAllText(this.caminhoListaFii, string.Empty); //override cache text
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Verifica a existência de cache para a 
        /// lista de resumo etalhamento de FII's
        /// </summary>
        public void LimparCacheDetalhamento()
        {
            try
            {
                File.WriteAllText(this.caminhoDetalhamentoFii, string.Empty); //override cache text
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Salvar cache da lista de FII        
        /// </summary>
        public void SalvarCacheLista(List<FII> list)
        {
            try
            {
                var json = JsonConvert.SerializeObject(list);
                File.WriteAllText(this.caminhoListaFii, json); //write string to file
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// salvar cache da lista de detalhamento de FII's        
        /// </summary>
        public void SalvarCacheDetalhamento(FIIDetalhado fii)
        {
            try
            {
                var dadosArquivo = File.ReadAllText(this.caminhoDetalhamentoFii);
                var listaDadosDetalhamento = JsonConvert.DeserializeObject<List<FIIDetalhado>>(dadosArquivo);

                if (listaDadosDetalhamento == null)
                    listaDadosDetalhamento = new List<FIIDetalhado>();
                listaDadosDetalhamento.Add(fii);

                var json = JsonConvert.SerializeObject(listaDadosDetalhamento);
                File.WriteAllText(this.caminhoDetalhamentoFii, json); //write string to file
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retornar dados do cache da lista de resumo dos FII's
        /// </summary>
        public async Task<List<FII>> RetornarDadosDeCacheLista()
        {
            try
            {
                var cache = File.ReadAllText(this.caminhoListaFii);
                var lista = JsonConvert.DeserializeObject<List<FII>>(cache);
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retornar dados do cache da lista de detalhamento de FII's        
        /// </summary>
        public async Task<FIIDetalhado> RetornarDadosDeCacheDetalhamento(string fiiBusca)
        {
            try
            {
                var cacheDetalhamento = File.ReadAllText(this.caminhoDetalhamentoFii);
                var listaDetalhamento = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cacheDetalhamento);
                FIIDetalhado fiiDesejado = new FIIDetalhado { codigoFii = "", nomeCompleto = "", cota = "", variacao = "", valorizacao = "", };

                // buscar FII dentro do cache
                if (listaDetalhamento != null)
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
                    var cacheLista = File.ReadAllText(this.caminhoDetalhamentoFii);
                    var lista = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cacheDetalhamento);
                    bool encontrouFiiNaLista = false;

                    if (lista != null)
                        foreach (var fii in lista)
                            if (fii.codigoFii.ToLower() == fiiBusca.ToLower())
                                encontrouFiiNaLista = true;

                    if (!encontrouFiiNaLista)
                    {
                        var crawler = new WebScraper.Crawler();
                        fiiDesejado = await crawler.CrawlInformacaoFIIAsync(fiiBusca);
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

        public async Task<List<FIIDetalhado>> RetornarListaDadosDeCacheDetalhamento(string userId)
        {
            try
            {
                var cacheDetalhamento = File.ReadAllText(this.caminhoDetalhamentoFii);
                List<FIIDetalhado> listaDetalhamento = JsonConvert.DeserializeObject<List<FIIDetalhado>>(cacheDetalhamento);
                listaDetalhamento.Find(item => item.userId == userId);                                
                return listaDetalhamento;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion        
    }
}