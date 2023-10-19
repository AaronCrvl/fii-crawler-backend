using fiiCrawlerApi.DbContexts;
using fiiCrawlerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Repositories
{
    public class UsuarioRepositorie
    {
        #region Propriedades
        public int Id { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }
        public string Categoria { get; set; }
        #endregion


        #region Métodos
        private static void ValidarDadosUsuario(Usuario usuario)
        {
            if (usuario.username.Length <= 1)
                throw new System.Exception("Este username não é válido");

            if(usuario.senha.Length < 5)
                throw new System.Exception("A senha deve conter 8 ou mais caracteres");

            List<char> caracteresInvalidos = new List<char> { '@', '#', '!', '%', '&', '*', '(', ')', '+', '-', '/', };
            foreach (char caractere in caracteresInvalidos)
                if(usuario.username.Contains(caractere))
                    throw new System.Exception("O username escolhido contém caracteres inválidos.");
        }

        public async Task<Usuario> BuscarUsuarioAsync(int? id, string username)
        {
            if (username.Length <= 1)
                throw new System.Exception("Este username não é válido");

            UsuarioContext contexto = new UsuarioContext();
            if (id == null)
                return await contexto.GetUsuario(null, username);
            else
                return await contexto.GetUsuario(id, username);
        }       

        public async Task<Usuario> CriarUsuario(Usuario usuarioASerCriado)
        {
            UsuarioContext contexto = new UsuarioContext();
            Usuario novoUsuario = await contexto.GetUsuario(null, usuarioASerCriado.username);
            return novoUsuario;
        }

        public async Task<Usuario> EditarUsuario(Usuario usuarioASerEditado)
        {
            ValidarDadosUsuario(usuarioASerEditado);
            UsuarioContext contexto = new UsuarioContext();
            Usuario usuarioEditado = await contexto.GetUsuario(null, usuarioASerEditado.username);
            return usuarioEditado;
        }

        public async Task<bool> ExcluirUsuario(Usuario usuarioASerCriado)
        {
            UsuarioContext contexto = new UsuarioContext();
            return await contexto.ExcluirUsuario(usuarioASerCriado.id);
        }
        #endregion
    }
}
