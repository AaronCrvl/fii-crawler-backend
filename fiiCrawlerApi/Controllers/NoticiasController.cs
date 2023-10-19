using fiiCrawlerApi.Models;
using fiiCrawlerApi.WebScraper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Controllers
{
    public class NoticiasController : Controller
    {
        [HttpGet]
        [Authorize]
        [Route("noticiasDaCarteira")]
        public async Task<ActionResult> getNewsAsync(string[] codigos)
        {
            try
            {
                var crawler = new Crawler();
                var lista = await crawler.CrawlNoticiasFIIAsync(codigos);

                return new ContentResult
                {
                    StatusCode = (int)System.Net.HttpStatusCode.OK,
                    ContentType = "application/json",
                    Content = System.Text.Json.JsonSerializer.Serialize(lista),
                };
            }
            catch (System.Exception)
            {
                return new ContentResult
                {
                    StatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
                    ContentType = "application/json",
                    Content = "Ocorreu algum problema ao retornar as noticias.",
                };
            }
        }
    }
}
