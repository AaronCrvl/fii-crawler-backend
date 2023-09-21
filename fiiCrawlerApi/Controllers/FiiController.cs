using fiiCrawlerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Controllers
{
    public class FiiController : Controller
    {
        [HttpGet]
        //[EnableCors()]
        [Route("getListaResumo/")]
        //https://localhost:44393/Fii/getListaResumo/
        public ActionResult getListaResumo()
        {
            try
            {

                var cache = new fiiCrawlerApi.Cache.GerenciadorDeCache();
                var crawler = new fiiCrawlerApi.WebScraper.Crawler();
                List<Fii> dados = crawler.GetListaResumoFii().Result;
                cache.SalvarCache(dados);

                
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                var res400 = new ContentResult();
                res400.Content = "Something went wrong.";
                res400.ContentType = "application/json";
                //res400.ContentEncoding = System.Text.Encoding.UTF8;
                return res400;
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
