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
        public string _path = Path.Combine(Environment.CurrentDirectory, @"Cache\cache.json");
        public DateTime _ultimaModificacao = File.GetLastWriteTime(Path.Combine(Environment.CurrentDirectory, @"Cache\cache.json"));
        #endregion

        #region Propriedades
        public string path
        {
            get { return this._path; }
            set { this._path = value; }
        }

        public DateTime ultimaModificacao
        {
            get { return this._ultimaModificacao; }
            set { this._ultimaModificacao = value; }
        }
        #endregion

        #region Construtores
        public GerenciadorDeCache() { }
        #endregion

        #region Métodos
        public bool ExisteCache()
        {
            var res = false;
            if (File.Exists(path) && File.ReadAllText(path).Length > 0)
                res = true;

            return res;
        }

        public void LimparCache()
        {
            try
            {
                if (ExisteCache())
                    System.IO.File.WriteAllText(this.path, string.Empty); //override cache text
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SalvarCache(List<Fii> list)
        {
            try
            {
                var json = JsonConvert.SerializeObject(list);
                System.IO.File.WriteAllText(this.path, json); //write string to file
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Fii>> RetornarDadosDeCache()
        {
            try
            {
                var cache = System.IO.File.ReadAllText(this.path);
                var lista = JsonConvert.DeserializeObject<List<Fii>>(cache);
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion        
    }
}