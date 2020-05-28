using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioMarcas : RepositorioBase, IRepositorioMarcas
    {
        public RepositorioMarcas() : base()
        {
        }

        public RepositorioMarcas(IDbTransaction transaction): base(transaction)
        {
        }

        public virtual Marca BuscarPorCodigo(string codigoMarca)
        {
            if (codigoMarca == null)
                return null;

            string query = @"SELECT MAR_CODIGO as Codigo , MAR_DESCRIPCION as Descripcion
                             FROM MARCA
                             WHERE MAR_CODIGO = @CodigoMarca";

            return Connection.QueryFirstOrDefault<Marca>(query, new { CodigoMarca = codigoMarca }, transaction: Transaction);
        }

        public virtual IEnumerable<Marca> ObtenerMarcas()
        {
            string query = @"SELECT MAR_CODIGO as Codigo , MAR_DESCRIPCION as Descripcion
                             FROM MARCA
                             ORDER BY MAR_DESCRIPCION";

            return Connection.Query<Marca>(query, transaction: Transaction);
        }

        public virtual IEnumerable<Marca> ObtenerMarcasPorRubro(int codigoRubro)
        {
            string query = @"SELECT DISTINCT m.MAR_CODIGO as Codigo , m.MAR_DESCRIPCION as Descripcion
                             FROM MARCA m
                             INNER JOIN STOCK s ON s.MAR_CODIGO = m.MAR_CODIGO
                             WHERE s.RUB_CODIGO = @CodigoRubro
                             ORDER BY MAR_DESCRIPCION";

            return Connection.Query<Marca>(query, new { CodigoRubro = codigoRubro }, transaction: Transaction);
        }
    }
}