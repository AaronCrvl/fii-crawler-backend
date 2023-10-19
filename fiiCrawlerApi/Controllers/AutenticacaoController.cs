using fiiCrawlerApi.Autenticacao.Seguranca;
using fiiCrawlerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Controllers
{
    [Route("v1/autenticacao")]
    public class AutenticacaoController : Controller
    {
        /// <summary>
        /// Valida o usuário dentro dos logins do sistema
        /// e retorna a chave baseada na autenticação JWT para o usuário
        /// </summary>
        [HttpGet]
        [Route("autenticar")]           
        public ActionResult Autenticar([FromBody] Usuario model)
        {
            try
            {
                var usuario = new Usuario { id = model.id, username = model.username, senha = model.senha, categoria = model.categoria };
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = Token.GenerateToken(usuario),
                    ContentType = "application/json",
                };
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = JsonConvert.SerializeObject(ex.Message),
                    ContentType = "application/json",
                };
            }
        }
    }
}
