using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Proxys;
using AutocorApi.Repositorios.Utils;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioProductos : RepositorioBase, IRepositorioProductos
    {
        private static readonly string _QueryProducto;
        private static readonly string _QueryProductoBase;

        static RepositorioProductos()
        {
            _QueryProducto = @" SELECT 
                                   -- {{top}} TOP(@Top)
                                   s.CODPIEZA as CodigoPieza
                                  ,s.NROORIGINAL as NumeroOriginal
                                  ,s.ARTICULO as Articulo
                                  ,s.DESCRIP as Descripcion
                                  , CASE WHEN s.UVTA > 0 THEN s.UVTA ELSE 1 END as UnidadVenta
                                  ,s.PRECIO as Precio
                                  ,s.OFERTA_TIPO as OfertaTipo
                                  ,s.OFERTA_IMPORTE as OfertaImporte
                                  ,s.OFERTA_VALIDEZ_HASTA as OfertaValidez
                                  ,s.OFERTA_SIGNO as OfertaSigno
                                  ,s.STOCK_ACTUAL as Stock
                                  ,s2.STOCK_ACTUAL as StockMadre
                                  ,s.CODPIEZA_MADRE as CodigoProductoMadre

                                  ,s.RUB_CODIGO as CodigoRubro
                                  ,r.RUB_CODIGO as Codigo
                                  ,r.RUB_DESCRIPCION as Descripcion
                                  ,r.RUB_FECHA as Fecha
                                  ,r.RUB_DESCUENTO as Descuento

                                  ,s.MAR_CODIGO as CodigoMarca
                                  ,m.MAR_CODIGO as Codigo
                                  ,m.MAR_DESCRIPCION as Descripcion

                                  ,s.TIP_CODIGO as CodigoTipoAuto
                                  ,t.TIP_CODIGO as Codigo
                                  ,t.TIP_DESCRIPCION as Descripcion
                              FROM STOCK s
                              INNER JOIN MARCA m ON s.MAR_CODIGO = m.MAR_CODIGO
                              INNER JOIN TIPO_AUTO t ON s.TIP_CODIGO = t.TIP_CODIGO
                              INNER JOIN RUBRO r ON s.RUB_CODIGO = r.RUB_CODIGO
                              LEFT JOIN STOCK s2 ON s.CODPIEZA_MADRE = s2.CODPIEZA ";

            _QueryProductoBase = @" SELECT 
                                   s.CODPIEZA as CodigoPieza
                                  ,s.NROORIGINAL as NumeroOriginal
                                  ,s.ARTICULO as Articulo
                                  ,s.DESCRIP as Descripcion
                              FROM STOCK s ";
        }

        public RepositorioProductos() : base()
        {
        }

        public RepositorioProductos(IDbTransaction transaction) : base(transaction)
        {
        }

        // private methods
        private Producto MapearProducto(Producto producto, Rubro rubro, Marca marca, TipoAuto tipoAuto, bool incluirValoresParametros = false)
        {
            tipoAuto.CodigoMarca = marca.Codigo;
            tipoAuto.Marca = marca;

            producto.CodigoMarca = marca.Codigo;
            producto.Marca = marca;

            producto.CodigoTipoAuto = tipoAuto.Codigo;
            producto.TipoAuto = tipoAuto;

            producto.CodigoRubro = rubro.Codigo;
            producto.Rubro = rubro;

            if (incluirValoresParametros)
            {
                // CUIDADO: se ejecuta por cada registro
                producto.Parametros = ObtenerParametrosProducto(producto.CodigoPieza);
            }

            return producto;
        }

        private IEnumerable<Producto> _ObtenerProductos(string query, DynamicParameters parametros)
        {
            return Connection.Query<ProductoProxy, RubroProxy, Marca, TipoAuto, Producto>(query,
                (productoAux, rubro, marca, tipoAuto) => MapearProducto(productoAux, rubro, marca, tipoAuto),
                parametros, transaction: Transaction, splitOn: "CodigoRubro,CodigoMarca,CodigoTipoAuto");
        }

        private IEnumerable<Producto> _ObtenerProductos(string codigoPieza = null, string[] codigosPiezas = null,
            string descripcion = null, int? codigoRubro = null, string codigoMarca = null, int? codigoTipoAuto = null,
            bool soloEnOferta = false, bool soloIncorporados = false,
            bool incluirParametros = false, PageConfig page = null)
        {
            IEnumerable<Producto> productos;
            var parametros = new DynamicParameters();

            string joinStockGrande = string.Empty;

            string query = _QueryProducto;

            // crear query
            var queryBuilder = CrearWhereObtenerProductos(query, parametros, codigoPieza, codigosPiezas, descripcion, codigoRubro, codigoMarca, codigoTipoAuto, soloEnOferta, soloIncorporados);

            // agregar orden
            queryBuilder.Append(" ORDER BY r.RUB_DESCRIPCION, m.MAR_DESCRIPCION, t.TIP_DESCRIPCION, s.DESCRIP ");

            // agregar paginación
            if (page != null)
            {
                queryBuilder.Append("OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ");
                parametros.AddDynamicParams(new {  page.Offset,  page.PageSize });
            }

            productos = _ObtenerProductos(queryBuilder.ToString(), parametros);

            if (incluirParametros && productos != null && productos.Count() > 0)
            {
                string[] codigosPiezasResultado = productos.Select(p => p.CodigoPieza).ToArray();
                var valoresParametros = ObtenerParametrosProductos(codigosPiezasResultado);

                foreach (var p in productos)
                {
                    p.Parametros = valoresParametros.Where(vp => vp.CodigoPieza == p.CodigoPieza);
                }
            }

            return productos;
        }

        private int _CountObtenerProductos(string codigoPieza, string[] codigosPiezas,
            string descripcion, int? codigoRubro, string codigoMarca,
            int? codigoTipoAuto, bool soloEnOferta, bool soloIncorporados)
        {
            var parametros = new DynamicParameters();
            string query = @"SELECT COUNT(*) FROM STOCK s ";

            query = CrearWhereObtenerProductos(query, parametros, codigoPieza, codigosPiezas, descripcion, codigoRubro, codigoMarca, codigoTipoAuto,
                soloEnOferta, soloIncorporados)
                .ToString();

            return Connection.ExecuteScalar<int>(query, parametros, transaction: Transaction);
        }

        private StringBuilder CrearWhereObtenerProductos(string queryInicial, DynamicParameters parametros,
            string codigoPieza, string[] codigosPiezas,
            string descripcion, int? codigoRubro, string codigoMarca,
            int? codigoTipoAuto, bool soloEnOferta, bool soloIncorporados)
        {
            StringBuilder queryBuilder = new StringBuilder(queryInicial);

            // extra joins
            bool joinStockGrande = false;
            //string joinStockGrande = string.Empty;
            queryBuilder.Append(" {{join_stock_grande}} ");

            // start where

            queryBuilder.Append("WHERE 1 = 1 ");

            if (!string.IsNullOrEmpty(codigoPieza))
            {
                queryBuilder.Append(" AND s.CODPIEZA = @CodigoPieza ");
                parametros.Add("CodigoPieza", codigoPieza);
            }

            if (codigosPiezas != null && codigosPiezas.Length > 0)
            {
                queryBuilder.Append(" AND s.CODPIEZA IN @CodigosPiezas ");
                parametros.Add("CodigosPiezas", codigosPiezas);
            }

            if (!string.IsNullOrEmpty(descripcion))
            {
                //joinStockGrande = " INNER JOIN stock_grande sg ON s.CODPIEZA = sg.codpieza ";
                joinStockGrande = true;
                string[] clavesBusqueda = descripcion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < clavesBusqueda.Length; i++)
                {
                    string claveBusqueda = clavesBusqueda[i];
                    string nombreParametro = "Key" + i;
                    queryBuilder.Append(" AND sg.descri LIKE @").Append(nombreParametro).Append(" ");   // ej. AND sg.descri LIKE @Key0
                    parametros.Add(nombreParametro, string.Format("%{0}%", claveBusqueda));
                }
            }

            if (codigoRubro.HasValue && codigoRubro > 0)
            {
                queryBuilder.Append(" AND s.RUB_CODIGO = @CodigoRubro ");
                parametros.Add("CodigoRubro", codigoRubro);
            }

            if (!string.IsNullOrEmpty(codigoMarca))
            {
                queryBuilder.Append(" AND s.MAR_CODIGO = @CodigoMarca ");
                parametros.Add("CodigoMarca", codigoMarca);
            }

            if (codigoTipoAuto.HasValue && codigoTipoAuto > 0)
            {
                 // queryBuilder.Append(" AND s.TIP_CODIGO = @CodigoTipoAuto ");
                queryBuilder.Append(" AND (s.TIP_CODIGO = @CodigoTipoAuto OR s.DESCRIP LIKE '%' + dbo.fn_TipoAutoDescripcion(@CodigoTipoAuto) + '%') ");
                parametros.Add("CodigoTipoAuto", codigoTipoAuto);
            }

            if (soloEnOferta)
            {
                queryBuilder.Append(" AND s.OFERTA_VALIDEZ_HASTA IS NOT NULL ");
                queryBuilder.Append(" AND s.OFERTA_VALIDEZ_HASTA >= DATEADD(DAY, -1, GETDATE()) ");
                queryBuilder.Append(" AND s.OFERTA_IMPORTE IS NOT NULL ");
            }

            if (soloIncorporados)
            {
                queryBuilder.Append(" AND s.NOVEDAD = @Novedad ");
                parametros.Add("Novedad", "A");
            }

            if(joinStockGrande)
            {
                queryBuilder.Replace("{{join_stock_grande}}", Environment.NewLine + " INNER JOIN stock_grande sg ON s.CODPIEZA = sg.codpieza " + Environment.NewLine);
            }
            else
            {
                queryBuilder.Replace("{{join_stock_grande}}", string.Empty);
            }

            return queryBuilder;
        }

        // interface methods
        public Producto BuscarPorCodigo(string codigoPieza, bool incluirParametros = true)
        {
            return this._ObtenerProductos(codigoPieza: codigoPieza, incluirParametros: incluirParametros).SingleOrDefault();
        }

        public IEnumerable<Producto> ObtenerProductos(string[] codigosPiezas, bool incluirParametros = false, PageConfig config = null)
        {
            return this._ObtenerProductos(
                codigosPiezas: codigosPiezas,
                incluirParametros: incluirParametros);
        }

        public virtual IEnumerable<ParametroProducto> ObtenerParametrosProducto(string codigoPieza)
        {
            return ObtenerParametrosProductos(new string[] { codigoPieza });
        }

        public IEnumerable<ParametroProducto> ObtenerParametrosProductos(string[] codigosPieza)
        {
            IList<ParametroProducto> valores = new List<ParametroProducto>();

            string query = @"SELECT s.CODPIEZA as CodigoPieza, s.RUB_CODIGO as CodigoRubro, r.RUB_CANT_PARA as CantidadParametros
	                        , rp.PARA1_N as Param1Descripcion, s.PARA1 as Param1Valor
	                        , rp.PARA2_N as Param2Descripcion, s.PARA2 as Param2Valor
	                        , rp.PARA3_N as Param3Descripcion, s.PARA3 as Param3Valor
	                        , rp.PARA4_N as Param4Descripcion, s.PARA4 as Param4Valor
	                        , rp.PARA5_N as Param5Descripcion, s.PARA5 as Param5Valor
	                        , rp.PARA6_N as Param6Descripcion, s.PARA6 as Param6Valor
	                        , rp.PARA7_N as Param7Descripcion, s.PARA7 as Param7Valor
	                        , rp.PARA8_N as Param8Descripcion, s.PARA8 as Param8Valor
	                        , rp.PARA9_N as Param9Descripcion, s.PARA9 as Param9Valor
	                        , rp.PARA10_N as Param10Descripcion, s.PARA10 as Param10Valor
                        FROM STOCK s
                        INNER JOIN RUBRO r ON s.RUB_CODIGO = r.RUB_CODIGO
                        INNER JOIN RUBRO_PARAMETRO rp ON rp.RUB_CODIGO = r.RUB_CODIGO
                        WHERE s.CODPIEZA IN @CodigosPieza";

            var rows = Connection.Query(query, new { CodigosPieza = codigosPieza }, transaction: Transaction) as IEnumerable<IDictionary<string, object>>;

            foreach (var row in rows)
            {
                if (row != null)
                {
                    string codigoPieza = row["CodigoPieza"] as string;
                    int cantidadParametros = row["CantidadParametros"] as int? ?? 0;

                    for (int i = 1; i <= cantidadParametros; i++)
                    {
                        string keyParametroDescripcion = string.Format("Param{0}Descripcion", i);
                        string keyParametroValor = string.Format("Param{0}Valor", i);

                        string nombreParametro = row[keyParametroDescripcion] as string ?? string.Empty;
                        string valorParametro = row[keyParametroValor] as string ?? string.Empty;

                        valores.Add(new ParametroProducto(codigoPieza, i, nombreParametro, valorParametro));
                    }
                }
            }

            return valores;
        }

        public IEnumerable<Producto> ObtenerProductos(string descripcion, int? codigoRubro, string codigoMarca, int? codigoTipoAuto, bool enOferta = false, bool incluirParametros = false, PageConfig config = null)
        {
            return this._ObtenerProductos(
                codigoPieza: null,
                descripcion: descripcion,
                codigoRubro: codigoRubro, codigoMarca: codigoMarca,
                codigoTipoAuto: codigoTipoAuto,
                soloEnOferta: enOferta,
                incluirParametros: incluirParametros,
                page: config);
        }

        public IEnumerable<Producto> ObtenerProductosEnOferta(bool incluirParametros = false)
        {
            return this._ObtenerProductos(
                soloEnOferta: true,
                incluirParametros: incluirParametros);
        }

        public IEnumerable<Producto> ObtenerProductosEnOfertaPorRubro(int codigoRubro, bool incluirParametros = false)
        {
            return this._ObtenerProductos(
                codigoRubro: codigoRubro,
                soloEnOferta: true,
                incluirParametros: incluirParametros);
        }

        public IEnumerable<Producto> ObtenerProductosEquivalentes(string codigoPieza, bool incluirParametros = false)
        {
            var parametros = new DynamicParameters();

            string query = _QueryProducto;

            query += @" INNER JOIN STOCK_EQUIVALENCIA se ON s.CODPIEZA = se.CODPIEZA_E
                        WHERE se.CODPIEZA = @CodigoPiezaEquivalencia
                        ORDER BY r.RUB_DESCRIPCION, m.MAR_DESCRIPCION, t.TIP_DESCRIPCION, s.DESCRIP";

            parametros.Add("CodigoPiezaEquivalencia", codigoPieza);

            return _ObtenerProductos(query, parametros);
        }

        public IEnumerable<Producto> ObtenerProductosIncorporados(bool incluirParametros = false)
        {
            return this._ObtenerProductos(
                codigoRubro: null,
                soloIncorporados: true,
                incluirParametros: incluirParametros);
        }

        public IEnumerable<Producto> ObtenerProductosIncorporadosPorRubro(int codigoRubro, bool incluirParametros = false)
        {
            return this._ObtenerProductos(
                codigoRubro: codigoRubro,
                soloIncorporados: true,
                incluirParametros: incluirParametros);
        }

        public virtual int ObtenerStockActual(string codigoPieza)
        {
            int? stock = null;

            string query = @"SELECT
                                CASE
		                            WHEN s.CODPIEZA_MADRE IS NOT NULL
                                        THEN (SELECT STOCK_ACTUAL FROM STOCK WHERE CODPIEZA = s.CODPIEZA_MADRE)
    		                        ELSE s.STOCK_ACTUAL
		                        END
                          FROM STOCK s
                          WHERE CODPIEZA = @CodigoPieza";

            stock = Connection.ExecuteScalar<int?>(query, new { CodigoPieza = codigoPieza }, transaction: Transaction);

            return stock ?? -1;
        }

        public virtual bool VerificarExistenciaDeProductosEquivalentes(string codigoPieza)
        {
            bool res = false;

            string query = @"SELECT CODPIEZA as CodigoPieza, CODPIEZA_E as CodigoPiezaEquivalente
                             FROM STOCK_EQUIVALENCIA
                             WHERE CODPIEZA = @CodigoPieza";

            var results = Connection.Query(query, new { CodigoPieza = codigoPieza }, transaction: Transaction);
            res = results.Count() > 0;

            return res;
        }

        public int CountObtenerProductos(string[] codigosPiezas)
        {
            return this._CountObtenerProductos(null, codigosPiezas, null, null, null, null, false, false);
        }

        public int CountObtenerProductos(string descripcion, int? codigoRubro, string codigoMarca, int? codigoTipoAuto, bool enOferta = false)
        {
            return this._CountObtenerProductos(null, null, descripcion, codigoRubro, codigoMarca, codigoTipoAuto, enOferta, false);
        }

        public IEnumerable<string> VerificarExistencia(string[] codigosPiezas)
        {
            string query = @"SELECT s.CODPIEZA FROM STOCK s WHERE s.CODPIEZA IN @CodigosPiezas";

            var existentes = Connection.Query<string>(query, 
                new { CodigosPiezas = codigosPiezas }, transaction: Transaction);

            return codigosPiezas.Where(x => !existentes.Any(x2 => x2 == x));
        }

        public bool VerificarExistencia(string codigoPieza)
        {
            string query = @"SELECT 1 FROM STOCK s WHERE s.CODPIEZA = @CodigoPieza";

            return Connection.Query(query, new { CodigoPieza = codigoPieza }, transaction: Transaction)
                .Count() > 0;
        }

        public IEnumerable<string> VerificarExistenciaDeGrupoProductosEquivalentes(string[] codigosPiezas)
        {
            if (codigosPiezas == null || codigosPiezas.Length == 0)
                return Enumerable.Empty<string>();

            string query = @"SELECT CODPIEZA
                             FROM STOCK_EQUIVALENCIA
                             WHERE CODPIEZA IN @codigosPiezas";

            return Connection.Query<string>(query, new { codigosPiezas }, transaction: Transaction);
        }

        public IEnumerable<Producto> ObtenerRandoms(int cantidad)
        {
            string query = _QueryProducto.Replace("-- {{top}}", string.Empty) +
                " ORDER BY NEWID() ";

            //return Connection.Query<Producto>(query, new { Top = cantidad }, transaction: Transaction);

            return Connection.Query<ProductoProxy, RubroProxy, Marca, TipoAuto, Producto>(query,
               (productoAux, rubro, marca, tipoAuto) => MapearProducto(productoAux, rubro, marca, tipoAuto),
               param: new { Top = cantidad }, transaction: Transaction, splitOn: "CodigoRubro,CodigoMarca,CodigoTipoAuto");
        }

        public IEnumerable<PrecioProducto> ObtenerPreciosProductos(string[] codigosPiezas)
        {
            string query = @"SELECT 
                                   s.CODPIEZA as CodigoPieza
                                  ,s.PRECIO as Precio
                                  ,s.OFERTA_TIPO as OfertaTipo
                                  ,s.OFERTA_IMPORTE as OfertaImporte
                                  ,s.OFERTA_VALIDEZ_HASTA as OfertaValidez
                                  ,s.OFERTA_SIGNO as OfertaSigno
                              FROM STOCK s
                              WHERE s.CODPIEZA IN @codigosPiezas";
            return Connection.Query<PrecioProducto>(query, new { codigosPiezas }, transaction: Transaction);
        }

        public IEnumerable<ProductoBase> ObtenerProductosBase(int? codigoRubro, string codigoMarca, int? codigoTipoAuto)
        {
            if (!codigoRubro.HasValue && string.IsNullOrEmpty(codigoMarca) && !codigoTipoAuto.HasValue)
                throw new Exception("Filtro de producto no especificado");

            DynamicParameters parametros = new DynamicParameters();
            string sql = _QueryProductoBase + "WHERE 1 = 1";

            if(codigoRubro.HasValue)
            {
                parametros.Add("codigoRubro", codigoRubro.Value);
                sql += " AND s.RUB_CODIGO = @codigoRubro ";
            }

            if(!string.IsNullOrEmpty(codigoMarca))
            {
                parametros.Add("codigoMarca", codigoMarca);
                sql += " AND s.MAR_CODIGO = @codigoMarca ";
            }

            if(codigoTipoAuto.HasValue)
            {
                parametros.Add("codigoTipoAuto", codigoTipoAuto.Value);
                sql += " AND s.TIP_CODIGO = @codigoTipoAuto ";
            }

            sql += " ORDER BY s.DESCRIP ASC";

            return Connection.Query<ProductoBase>(sql, parametros, transaction: Transaction);
        }

        public virtual IEnumerable<Producto> ObtenerProductosMasPedidos(DateTime desde, DateTime hasta, int cantidad = 10, int? zona = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Desde", desde);
            parameters.Add("Hasta", hasta);
            parameters.Add("Top", cantidad);

            string query = _QueryProducto +
                @" INNER JOIN (SELECT dp.COD_PIEZA AS CodigoPieza, sum(dp.CANTIDAD) AS Cantidad
                                          FROM DETALLE_PEDIDO dp
                                          INNER JOIN PEDIDOS p ON dp.ID_PEDIDO = p.ID_PEDIDO
                                        -- {{zona}} INNER JOIN CLIENTES c2 ON c2.CODCLI = p.COD_CLIENTE
                                        WHERE p.FECHA BETWEEN @Desde AND @Hasta
                                            -- {{zona}} AND c.ZONA = @Zona
                                        GROUP BY dp.COD_PIEZA
                                        ) ranking ON s.CODPIEZA = ranking.CodigoPieza
                             ORDER BY ranking.Cantidad DESC";

            query = query.Replace("-- {{top}}", string.Empty);
            
            if(zona.HasValue)
            {
                query = query.Replace("-- {{zona}}", string.Empty);
            }

            return _ObtenerProductos(query, parameters);
        }
    }
}