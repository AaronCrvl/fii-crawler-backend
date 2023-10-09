using fiiCrawlerApi.Authorization.Security;
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
    public class AuthController : Controller
    {
        /// <summary>
        /// Valida o usuário dentro dos logins do sistema
        /// e retorna a chave baseada na autenticação JWT para o usuário
        /// </summary>
        [HttpGet]
        [Route("autenticar")]
        //https://localhost:44304/aunteticar       
        public ActionResult Autenticar([FromBody]Usuario model)
        {
            try
            {
                var usuario = new Usuario { Id = model.Id, Username = model.Username, Password = model.Password, Role = model.Role };

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return new ContentResult { Content = Token.GenerateToken(usuario), ContentType = "application/json" };
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                ContentResult res200 = new ContentResult();
                res200.Content = JsonConvert.SerializeObject(ex.Message);
                res200.ContentType = "application/json";
                return res200;
            }
        }
    }
}
