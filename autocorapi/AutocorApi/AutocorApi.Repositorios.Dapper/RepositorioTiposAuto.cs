using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Proxys;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioTiposAuto : RepositorioBase, IRepositorioTiposAuto
    {
        public RepositorioTiposAuto() : base()
        {
        }

        public RepositorioTiposAuto(IDbTransaction transaction) : base(transaction)
        {
        }

        public virtual TipoAutoBase BuscarBasePorCodigo(int codigoTipoAuto)
        {
            string query = @"SELECT t.TIP_CODIGO as Codigo,
                                    t.TIP_DESCRIPCION as Descripcion,
                                    t.MAR_CODIGO as CodigoMarca
                             FROM TIPO_AUTO t
                             WHERE t.TIP_CODIGO = @codigoTipoAuto ";

            return Connection.QueryFirstOrDefault<TipoAutoBase>(query, param: new { codigoTipoAuto }, transaction: Transaction);
        }

        public virtual TipoAuto BuscarPorCodigo(int codigoTipoAuto)
        {
            return this._ObtenerTiposAuto(codigoTipoAuto: codigoTipoAuto, codigoMarca: null).SingleOrDefault();
        }

        public IEnumerable<TipoAuto> ObtenerTiposAuto()
        {
            return this._ObtenerTiposAuto(codigoTipoAuto: null, codigoMarca: null);
        }

        public virtual IEnumerable<TipoAutoBase> ObtenerTiposAutoBase(string codigoMarca)
        {
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT t.TIP_CODIGO as Codigo,
                                    t.TIP_DESCRIPCION as Descripcion,
                                    t.MAR_CODIGO as CodigoMarca
                             FROM TIPO_AUTO t
                             WHERE 1 = 1 ";

            if(!string.IsNullOrEmpty(codigoMarca))
            {
                query += " AND t.MAR_CODIGO = @codigoMarca ";
                parameters.Add("codigoMarca", codigoMarca);
            }

            query += "ORDER BY t.TIP_DESCRIPCION";

            return Connection.Query<TipoAutoBase>(query, param: parameters, transaction: Transaction);
        }

        public IEnumerable<TipoAuto> ObtenerTiposAutoPorMarca(string codigoMarca)
        {
            return this._ObtenerTiposAuto(codigoTipoAuto: null, codigoMarca: codigoMarca);
        }

        // private methods
        private TipoAuto MapearTipoAuto(TipoAuto tipoAuto, Marca marca)
        {
            tipoAuto.Marca = marca;
            tipoAuto.CodigoMarca = marca.Codigo;
            return tipoAuto;
        }

        protected virtual IEnumerable<TipoAuto> _ObtenerTiposAuto(int? codigoTipoAuto = null, string codigoMarca = null)
        {
            var parametros = new DynamicParameters();

            string query = @"SELECT t.TIP_CODIGO as Codigo,
                                    t.TIP_DESCRIPCION as Descripcion,
                                    t.MAR_CODIGO as CodigoMarca,
                                    m.MAR_CODIGO as Codigo,
                                    m.MAR_DESCRIPCION as Descripcion
                             FROM TIPO_AUTO t
                             INNER JOIN MARCA m ON t.MAR_CODIGO = m.MAR_CODIGO
                             WHERE 1 = 1 ";

            if (codigoTipoAuto.HasValue)
            {
                query += " AND t.TIP_CODIGO = @CodigoTipoAuto ";
                parametros.Add("CodigoTipoAuto", codigoTipoAuto);
            }

            if (!string.IsNullOrEmpty(codigoMarca))
            {
                query += " AND t.MAR_CODIGO = @CodigoMarca ";
                parametros.Add("CodigoMarca", codigoMarca);
            }

            query += "ORDER BY t.TIP_DESCRIPCION";

            return Connection.Query<TipoAutoProxy, Marca, TipoAuto>(query,
                (tipoAuto, marca) => MapearTipoAuto(tipoAuto, marca),
                parametros, transaction: Transaction, splitOn: "CodigoMarca");
        }
    }
}