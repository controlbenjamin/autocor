using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;
using System.Data;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioRefreshTokens : RepositorioBase, IRepositorioRefreshTokens
    {
        #region Queries

        private const string _QueryRefreshToken =
           @"SELECT t.Id
                  ,t.IdUsuario
                  ,t.ClientId
                  ,t.IssuedUTC
                  ,t.ExpiresUTC
                  ,t.ProtectedTicket
              FROM TOKENS t ";

        private const string _QueryEliminarRefreshToken =
            "DELETE FROM TOKENS WHERE Id = @id";

        private const string _QueryInsertarRefreshToken =
            @"INSERT INTO TOKENS
                   (Id
                   ,IdUsuario
                   ,ClientId
                   ,IssuedUTC
                   ,ExpiresUTC
                   ,ProtectedTicket)
             VALUES
                   (@Id
                   ,@IdUsuario
                   ,@ClientId
                   ,@IssuedUTC
                   ,@ExpiresUTC
                   ,@ProtectedTicket)";

        #endregion Queries

        public RepositorioRefreshTokens() : base()
        {

        }

        public RepositorioRefreshTokens(IDbTransaction transaction) : base(transaction)
        {

        }

        public void Eliminar(string id, int? idUsuario = null)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("id", id);

            string sql = _QueryEliminarRefreshToken;

            if(idUsuario.HasValue)
            {
                sql += " AND IdUsuario = @idUsuario ";
                param.Add("idUsuario", idUsuario);
            }

            Connection.Execute(sql, param: param, transaction: Transaction);
        }

        public void Insertar(RefreshToken refreshToken)
        {
            Connection.Execute(_QueryInsertarRefreshToken, refreshToken, transaction: Transaction);
        }

        public RefreshToken ObtenerPorId(string id)
        {
            string sql = _QueryRefreshToken + " WHERE t.Id = @id ";
            return Connection.QueryFirstOrDefault<RefreshToken>(sql, param: new { id }, transaction: Transaction);
        }
    }
}