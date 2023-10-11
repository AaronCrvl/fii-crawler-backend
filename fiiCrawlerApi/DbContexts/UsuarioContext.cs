using fiiCrawlerApi.Models;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using System.Linq;
using fiiCrawlerApi.DbContexts.Records;

namespace fiiCrawlerApi.DbContexts
{
    public class UsuarioContext
    {
        #region Variáveis Privadas
        private string connectionString = "";
        #endregion

        #region Construtor
        public UsuarioContext()
        {
            this.connectionString = ConfigurationManager.AppSettings.AllKeys.ToList<string>().FirstOrDefault();
        }
        #endregion

        #region Métodos
        public async Task<bool> CriarUsuario(Usuario usuario)
        {
            try
            {
                await using (var connection = new SqlConnection(this.connectionString))
                {
                    await connection.ExecuteAsync("INSERT INTO [Usuario] VALUES(@username, @senha)",
                    new
                    {
                        username = usuario.username,
                        senha = usuario.senha,
                    });
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Models.Usuario> GetUsuario(string? id, string? username)
        {
            try
            {
                UsuarioDB usuarioDb;
                if (id != null)
                    await using (var connection = new SqlConnection(this.connectionString))
                        usuarioDb = await connection.QueryFirstOrDefaultAsync<UsuarioDB>("SELECT [Id], [username], [Senha] FROM [Category] WHERE [Id]=@id", new { id = id });
                else
                    await using (var connection = new SqlConnection(this.connectionString))
                        usuarioDb = await connection.QueryFirstOrDefaultAsync<UsuarioDB>("SELECT [Id], [username], [Senha] FROM [Usuario] WHERE [username]=@username", new { username = username });

                Usuario usuario;
                usuario = usuarioDb == null ?
                    new Models.Usuario { id = 0, username = "", senha = "", }
                    : new Models.Usuario { id = usuarioDb.id, username = usuarioDb.username, senha = usuarioDb.senha, categoria = usuarioDb.categoria, tipo = usuarioDb.tipo };

                return usuario;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> EditarUsuario(Usuario usuario)
        {
            try
            {
                await using (var connection = new SqlConnection(this.connectionString))
                {
                    await connection.ExecuteAsync("UPDATE [Usuario] SET [Username]=@username, [Senha]=@senha, [Categoria]=@categoria, [Tipo]=@tipo, WHERE [Id]=@id",
                        new { id = usuario.id, username = usuario.username, senha = usuario.senha, Categoria = usuario.categoria, tipo = usuario.tipo });
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> ExcluirUsuario(int idUsuario)
        {
            try
            {
                await using (var connection = new SqlConnection(this.connectionString))
                {
                    await connection.ExecuteAsync("DELETE FROM [Usuario] WHERE [Id]=@id",
                        new { id = idUsuario });
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion        
    }
}
