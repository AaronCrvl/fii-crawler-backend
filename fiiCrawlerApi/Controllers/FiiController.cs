using fiiCrawlerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Controllers
{
    public class FiiController : Controller
    {
        [HttpGet]
        //[EnableCors()]
        [Route("getListaResumo/")]
        //https://localhost:44304/Fii/getListaResumo/
        public ActionResult getListaResumo()
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
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        //[EnableCors()]
        [Route("getFII/{fiiBusca}")]
        //https://localhost:44304/Fii/getFII/{fiiBusca}
        public ActionResult getFiiAsync(string fiiBusca)
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
                FIIDetalhado fii = cache.RetornarDadosDeCacheDetalhamento(fiiBusca).Result;
                
                if(fii.codigoFii == "")
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
                    res200.Content = JsonConvert.SerializeObject(fii);
                    res200.ContentType = "application/json";
                    return res200;
                }                
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
