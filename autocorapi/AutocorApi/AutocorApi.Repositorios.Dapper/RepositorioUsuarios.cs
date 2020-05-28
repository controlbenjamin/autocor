using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioUsuarios : RepositorioBase, IRepositorioUsuarios
    {
        #region Queries

        private const string _QueryUsuarios =
             @"SELECT
                   IdUsuario,
                   Usuario as NombreUsuario
                  ,Nombre
                  ,FechaCreacionUtc
                  ,Activo
                  ,Rol
                  ,Password
                  ,CodigoCliente
              FROM UsuariosGlobal ";

        #endregion

        public RepositorioUsuarios() : base()
        {
        }

        public RepositorioUsuarios(IDbTransaction transaction) : base(transaction)
        {
        }

        public Usuario Buscar(string nombreUsuario)
        {
            string query = _QueryUsuarios + " WHERE Usuario = @nombreUsuario";
            return Connection.QuerySingleOrDefault<Usuario>(query, 
                new { nombreUsuario }, transaction: Transaction);
        }

        public Usuario Buscar(string nombreUsuario, string password)
        {
            string query = _QueryUsuarios + " WHERE Usuario = @nombreUsuario COLLATE Modern_Spanish_CS_AS AND Password = @password COLLATE Modern_Spanish_CS_AS";
            return Connection.QuerySingleOrDefault<Usuario>(query, 
                new { nombreUsuario, password }, transaction: Transaction);
        }

        public Usuario BuscarPorCodigoCliente(int codigoCliente)
        {
            string query = _QueryUsuarios + " WHERE CodigoCliente = @codigoCliente";
            return Connection.QuerySingleOrDefault<Usuario>(query,
                new { codigoCliente }, transaction: Transaction);
        }

        public Usuario Buscar(int idUsuario)
        {
            string query = _QueryUsuarios + " WHERE IdUsuario = @idUsuario";
            return Connection.QuerySingleOrDefault<Usuario>(query,
                new { idUsuario }, transaction: Transaction);
        }

        public int ObtenerZonaPorUsuario(int idUsuario, Rol? rol = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string query = "SELECT u.ZonaViajante FROM UsuariosGlobal u WHERE u.IdUsuario = @idUsuario";
            parameters.Add("idUsuario", idUsuario);

            if(rol != null)
            {
                query += " AND u.Rol = @rol";
                parameters.Add("rol", rol.ToString());
            }

            var zona = Connection.ExecuteScalar<int?>(query, param: parameters, transaction: Transaction);
            return zona ?? -1;
        }
    }
}