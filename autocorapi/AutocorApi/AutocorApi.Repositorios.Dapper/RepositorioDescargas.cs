using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioDescargas : RepositorioBase, IRepositorioDescargas
    {
        public RepositorioDescargas() : base()
        {
        }

        public RepositorioDescargas(IDbTransaction transaction) : base(transaction)
        {
        }

        public IEnumerable<Descarga> ObtenerDescargas()
        {
            string query = @"SELECT d.ID as Id
                              ,d.DESCRIPCION as Descripcion
                              ,d.ENLACE as Link
                          FROM DESCARGAS d
                          ORDER BY d.DESCRIPCION ";

            return Connection.Query<Descarga>(query, transaction: Transaction);
        }

        public Descarga ObtenerPorId(int idDescarga)
        {
            string query = @"SELECT d.ID as Id
                              ,d.DESCRIPCION as Descripcion
                              ,d.ENLACE as Link
                          FROM DESCARGAS d
                          WHERE d.ID = @IdDescarga";

            return Connection.QuerySingleOrDefault<Descarga>(query, new { IdDescarga = idDescarga }, transaction: Transaction);
        }
    }
}