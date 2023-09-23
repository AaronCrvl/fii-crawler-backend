using fiiCrawlerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace fiiCrawlerApi.Controllers
{
    public class FiiController : Controller
    {
        /// <summary>
        /// Retornar lista de FII's.        
        /// </summary>
        [HttpGet]
        //[EnableCors()]
        [Route("getListaResumo/")]
        //https://localhost:44304/Fii/getListaResumo/
        public ActionResult GetListaResumo()
        {
            try
            {
                /*
                 * CORS !_________________________________________
                 * 
                 * Headers necessários para evitar bloqueios do CORS.
                 * Neste caso são enviados headers específicos para permitir
                 * o acesso vindo de todas as origens externas a da api (https://localhost:44304/)
                 * e para o acesso ao tipo de método requisitado.
                */
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var cache = new fiiCrawlerApi.Cache.GerenciadorDeCache();

                List<FII> dados = new List<FII>();
                dados = cache.RetornarDadosDeCacheLista().Result;

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                ContentResult res200 = new ContentResult();
                res200.Content = JsonConvert.SerializeObject(dados);
                res200.ContentType = "application/json";
                return res200;
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ContentResult res500 = new ContentResult();
                res500.Content = ex.Message;
                return res500;
            }
        }

        /// <summary>
        /// Retornar informações detalhadas de um FII's específico.
        /// </summary>
        [HttpGet]
        //[EnableCors()]
        [Route("getFII/{fiiBusca}")]
        //https://localhost:44304/Fii/getFII/{fiiBusca}
        public ActionResult GetFii(string fiiBusca)
        {
            try
            {
                /*
                 * CORS !_________________________________________
                 * 
                 * Headers necessários para evitar bloqueios do CORS.
                 * Neste caso são enviados headers específicos para permitir
                 * o acesso vindo de todas as origens externas a da api (https://localhost:44304/)
                 * e para o acesso ao tipo de método requisitado.
                */
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var fundo = new FIIDetalhado();
                var cache = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                if (!cache.ExisteNoCacheDeDetalhamento(fiiBusca))
                {
                    fundo = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFII(fiiBusca).Result;
                    cache.SalvarCacheDetalhamento(fundo);
                }
                else
                    fundo = cache.RetornarDadosDeCacheDetalhamento(fiiBusca).Result;

                if (fundo.codigoFii != "")
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ContentResult res400 = new ContentResult();
                    res400.Content = $"O FII '{fiiBusca}' não foi encontrado na lista de Fundos de Investimento disponíveis.";
                    res400.ContentType = "application/json";
                    return res400;
                }
                else
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    ContentResult res200 = new ContentResult();
                    res200.Content = JsonConvert.SerializeObject(fundo);
                    res200.ContentType = "application/json";
                    return res200;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ContentResult res500 = new ContentResult();
                res500.Content = ex.Message;
                return res500;
            }
        }
    }
}
