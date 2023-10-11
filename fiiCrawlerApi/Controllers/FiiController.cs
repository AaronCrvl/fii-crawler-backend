using fiiCrawlerApi.Autenticacao.Seguranca;
using fiiCrawlerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Controllers
{
    [Route("v1/fii")]
    public class FIIController : Controller
    {
        /// <summary>
        /// Retornar lista de FII's.        
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("listaResumo")]
        //https://localhost:44304/v1/fii/listaResumo        
        public async Task<ActionResult> BuscarListaResumidaFundosInvestimento()
        {
            try
            {              
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
        [Route("buscarFundo/{fiiBusca}")]
        [Authorize]
        //https://localhost:44304/v1/fii/fiiBusca
        public async Task<ActionResult> BuscarFundoDeInvestimento([FromBody]string fiiBusca, string userId)
        {
            try
            {             
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var fundo = new FIIDetalhado();
                var cache = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                if (cache.ExisteNoCacheDeDetalhamento(fiiBusca, userId))
                    fundo = cache.RetornarDadosDeCacheDetalhamento(fiiBusca).Result;
                else
                {
                    fundo = new fiiCrawlerApi.WebScraper.Crawler().CrawlInformacaoFIIAsync(fiiBusca).Result;
                    fundo.userId = userId;
                    cache.SalvarCacheDetalhamento(fundo);
                }


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

        [HttpGet]
        [Authorize]
        [Route("listaFIIusuario")]
        //https://localhost:44304/v1/fii/listaFIIusuario
        public async Task<ActionResult> ListagemFIICarteiraUsuario([FromBody]string userID)
        {
            try
            {              
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var cache = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                List<FIIDetalhado> dados = new List<FIIDetalhado>();
                dados = cache.RetornarListaDadosDeCacheDetalhamento(userID).Result;

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
        /// Efetuar compra de uma quantidade de cotas de um FII
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("registrarCompra")]
        //https://localhost:44304/v1/fii/registrarCompra
        public async Task<ActionResult> RegistrarCompra([FromBody] Transacao transacao)
        {
            try
            {               
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                ContentResult res200 = new ContentResult();
                res200.Content = $"Compra de {transacao.qtdCotas} cotas fundo {transacao.codigoFII} registrada com sucesso!";
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
    }
}
