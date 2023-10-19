using fiiCrawlerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System;
using System.Threading.Tasks;
using fiiCrawlerApi.Repositories;
using System.Net.Mime;
using fiiCrawlerApi.DbContexts.Records;

namespace fiiCrawlerApi.Controllers
{
    [Route("v1/usuario")]
    public class UsuarioController : Controller
    {
        /// <summary>
        /// Retornar dados do usuário      
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("retornarUsuario")]
        //https://localhost:44304/v1/usuario/retornarUsuario        
        public async Task<ActionResult> getDadosUsuario([FromBody] int? id, string username)
        {
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var repo = new UsuarioRepositorie();
                var usuario = await repo.BuscarUsuarioAsync(id, username);

                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(usuario),
                };
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.InternalServerError,                    
                    Content = JsonConvert.SerializeObject(ex.Message),
                };                
            }
        }

        /// <summary>
        /// Altear dados do usuário      
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("alterarUsuario")]
        //https://localhost:44304/v1/usuario/alterarUsuario        
        public async Task<ActionResult> alterarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var repo = new UsuarioRepositorie();
                await repo.EditarUsuario(usuario);

                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(usuario),
                };
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(ex.Message),
                };
            }
        }

        /// <summary>
        /// Altear dados do usuário      
        /// </summary>
        [HttpDelete]
        [Authorize]
        [Route("excluirConta")]
        //https://localhost:44304/v1/usuario/excluirConta        
        public async Task<ActionResult> excluirUsuario([FromBody] Usuario usuario)
        {
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");

                var repo = new UsuarioRepositorie();
                if(await repo.ExcluirUsuario(usuario))
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject("Exclusão feita com sucesso."),
                };
                else
                    return new ContentResult
                    {
                        StatusCode = (int)HttpStatusCode.NotAcceptable,
                        ContentType = "application/json",
                        Content = JsonConvert.SerializeObject("Ocorreu algum problema ao tentar excluir a conta de usuário."),
                    };
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(ex.Message),
                };
            }
        }
    }
}
