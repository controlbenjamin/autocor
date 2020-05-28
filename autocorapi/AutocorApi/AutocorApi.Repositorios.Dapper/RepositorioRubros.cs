using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Proxys;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioRubros : RepositorioBase, IRepositorioRubros
    {

        #region Queries

        private const string _QueryRubroBase =
            @"SELECT r.RUB_CODIGO as Codigo
                    ,r.RUB_DESCRIPCION as Descripcion
              FROM RUBRO r ";

        #endregion


        public RepositorioRubros() : base()
        {
        }

        public RepositorioRubros(IDbTransaction transaction) : base(transaction)
        {
        }

        public virtual Rubro BuscarPorCodigo(int codigoRubro)
        {
            return this._ObtenerRubros(codigoRubro: codigoRubro).SingleOrDefault();
        }

        public virtual IEnumerable<string> ObtenerParametrosPorRubro(int codigoRubro)
        {
            IList<string> parametros = new List<string>();
            string query = @"SELECT r.RUB_CANT_PARA as CantidadParametros,
                                    rp.PARA1_N, rp.PARA2_N, rp.PARA3_N, rp.PARA4_N, rp.PARA5_N, rp.PARA6_N,
                                    rp.PARA7_N, rp.PARA8_N, rp.PARA9_N, rp.PARA10_N
                            FROM RUBRO_PARAMETRO rp
                            INNER JOIN RUBRO r ON r.RUB_CODIGO = rp.RUB_CODIGO
                            WHERE rp.RUB_CODIGO = @CodigoRubro ";

            var row = Connection.QueryFirstOrDefault(query, new { CodigoRubro = codigoRubro }, transaction: Transaction) as IDictionary<string, object>;
            int cantidadParametros = (int)row["CantidadParametros"];

            if (row != null)
            {
                for (int i = 1; i <= cantidadParametros; i++)
                {
                    string key = string.Format("PARA{0}_N", i);
                    string nombreParametro = row[key] as string ?? string.Empty;
                    parametros.Add(nombreParametro);
                }
            }

            return parametros;
        }

        public virtual RubroBase ObtenerRubroBasePorCodigo(int codigoRubro)
        {
            string query = _QueryRubroBase + " WHERE r.RUB_CODIGO = @codigoRubro";
            return Connection.QueryFirstOrDefault<RubroBase>(query, param: new { codigoRubro }, transaction: Transaction);
        }

        public virtual IEnumerable<Rubro> ObtenerRubros()
        {
            return this._ObtenerRubros(codigoRubro: null, codigoMarca: null, codigoTipoAuto: null);
        }

        public IEnumerable<Rubro> ObtenerRubros(string codigoMarca, string codigoTipoAuto)
        {
            return this._ObtenerRubros(codigoRubro: null, codigoMarca: codigoMarca, codigoTipoAuto: codigoTipoAuto);
        }

        public virtual IEnumerable<RubroBase> ObtenerRubrosBase()
        {
            string query = _QueryRubroBase + " ORDER BY r.RUB_DESCRIPCION ASC";
            return Connection.Query<RubroBase>(query, transaction: Transaction);
        }

        public IEnumerable<Rubro> ObtenerRubrosIncorporaciones()
        {
            IEnumerable<Rubro> rubros = new List<Rubro>();

            string query = @"SELECT DISTINCT r.RUB_CODIGO as Codigo
                              ,r.RUB_DESCRIPCION as Descripcion
                              ,r.RUB_FECHA as Fecha
                              ,r.RUB_CANT_PARA as CantidadParametros
                              ,r.RUB_DESCUENTO as Descuento
                              ,rp.PARA1_N, rp.PARA2_N, rp.PARA3_N, rp.PARA4_N, rp.PARA5_N, rp.PARA6_N,
                                    rp.PARA7_N, rp.PARA8_N, rp.PARA9_N, rp.PARA10_N
                              FROM RUBRO r
                              INNER JOIN RUBRO_PARAMETRO rp on r.RUB_CODIGO = rp.RUB_CODIGO
                              INNER JOIN STOCK s ON s.RUB_CODIGO = r.RUB_CODIGO
                              WHERE S.NOVEDAD = 'A'
                              ORDER BY r.RUB_DESCRIPCION ASC";

            var rows = Connection.Query(query, transaction: Transaction) as IEnumerable<IDictionary<string, object>>;
            rubros = CrearRubros(rows);
            return rubros;
        }

        public IEnumerable<Rubro> ObtenerRubrosOferta()
        {
            IEnumerable<Rubro> rubros = new List<Rubro>();

            string query = @"SELECT DISTINCT r.RUB_CODIGO as Codigo
                              ,r.RUB_DESCRIPCION as Descripcion
                              ,r.RUB_FECHA as Fecha
                              ,r.RUB_CANT_PARA as CantidadParametros
                              ,r.RUB_DESCUENTO as Descuento
                              ,rp.PARA1_N, rp.PARA2_N, rp.PARA3_N, rp.PARA4_N, rp.PARA5_N, rp.PARA6_N,
                              rp.PARA7_N, rp.PARA8_N, rp.PARA9_N, rp.PARA10_N
                      FROM RUBRO r
                      INNER JOIN RUBRO_PARAMETRO rp on r.RUB_CODIGO = rp.RUB_CODIGO
                      INNER JOIN STOCK s ON s.RUB_CODIGO = r.RUB_CODIGO
                      WHERE s.OFERTA_VALIDEZ_HASTA IS NOT NULL
                        AND s.OFERTA_VALIDEZ_HASTA >= DATEADD(DAY, -1, GETDATE())
                      ORDER BY r.RUB_DESCRIPCION ASC";

            var rows = Connection.Query(query, transaction: Transaction) as IEnumerable<IDictionary<string, object>>;
            rubros = CrearRubros(rows);
            return rubros;
        }

        // private methods
        private Rubro CrearRubro(IDictionary<string, object> row)
        {
            if (row == null)
                return null;

            IList<string> parametrosRubro = new List<string>();
            Rubro rubro = new RubroProxy
            {
                Codigo = (int)row["Codigo"],
                Descripcion = row["Descripcion"] as string,
                Descuento = row["Descuento"] as decimal?,
                Fecha = row["Fecha"] as DateTime?
            };

            int cantidadParametros = row["CantidadParametros"] as int? ?? 0;

            for (int i = 1; i <= cantidadParametros; i++)
            {
                string keyParametroDescripcion = string.Format("PARA{0}_N", i);
                string parametro = row[keyParametroDescripcion] as string ?? string.Empty;
                parametrosRubro.Add(parametro);
            }

            rubro.ListaParametros = parametrosRubro;

            return rubro;
        }

        private IEnumerable<Rubro> CrearRubros(IEnumerable<IDictionary<string, object>> rows)
        {
            if (rows == null)
                yield break;

            foreach (var row in rows)
            {
                if (row != null)
                    yield return CrearRubro(row);
            }
        }

        private IEnumerable<Rubro> _ObtenerRubros(int? codigoRubro = null, string codigoMarca = null, string codigoTipoAuto = null)
        {
            IEnumerable<Rubro> rubros = new List<Rubro>();
            var parametros = new DynamicParameters();

            bool joinStock = false;

            string query = @"SELECT DISTINCT r.RUB_CODIGO as Codigo
                              ,r.RUB_DESCRIPCION as Descripcion
                              ,r.RUB_FECHA as Fecha
                              ,r.RUB_CANT_PARA as CantidadParametros
                              ,r.RUB_DESCUENTO as Descuento
                              ,rp.PARA1_N, rp.PARA2_N, rp.PARA3_N, rp.PARA4_N, rp.PARA5_N, rp.PARA6_N,
                                    rp.PARA7_N, rp.PARA8_N, rp.PARA9_N, rp.PARA10_N
                      FROM RUBRO r
                      INNER JOIN RUBRO_PARAMETRO rp on r.RUB_CODIGO = rp.RUB_CODIGO
                      -- join_stock INNER JOIN STOCK s ON s.RUB_CODIGO = r.RUB_CODIGO
                      WHERE 1 = 1 ";

            if (codigoRubro.HasValue)
            {
                query += " AND r.RUB_CODIGO = @CodigoRubro ";
                parametros.Add("CodigoRubro", codigoRubro);
            }

            if (!string.IsNullOrEmpty(codigoMarca))
            {
                joinStock = true;
                query += " AND s.MAR_CODIGO = @CodigoMarca ";
                parametros.Add("CodigoMarca", codigoMarca);
            }

            if (!string.IsNullOrEmpty(codigoTipoAuto))
            {
                joinStock = true;
                query += " AND s.TIP_CODIGO = @CodigoTipoAuto ";
                parametros.Add("CodigoTipoAuto", codigoTipoAuto);
            }

            if (joinStock)
            {
                query = query.Replace("-- join_stock", "");
            }

            query += "ORDER BY r.RUB_DESCRIPCION ASC";

            var rows = Connection.Query(query, parametros, transaction: Transaction) as IEnumerable<IDictionary<string, object>>;
            rubros = CrearRubros(rows);

            return rubros;
        }
    }
}