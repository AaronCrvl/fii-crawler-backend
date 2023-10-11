using fiiCrawlerApi.DbContexts;
using fiiCrawlerApi.Models;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Repositories
{
    public class UserRepositorie
    {
        #region Propriedades
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        #endregion


        #region Métodos
        public async Task<Usuario> BuscarUsuarioAsync(string id) 
        {
            UsuarioContext contexto = new UsuarioContext();
            return await contexto.GetUsuario(null, id);
        }
        public async Task<Usuario> CriarUsuario(Usuario usuarioASerCriado)
        {
            UsuarioContext contexto = new UsuarioContext();
            Usuario novoUsuario = await contexto.GetUsuario(null, usuarioASerCriado.username);
            return novoUsuario;
        }

        public async Task<Usuario> EditarUsuario(Usuario usuarioASerEditado)
        {
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
