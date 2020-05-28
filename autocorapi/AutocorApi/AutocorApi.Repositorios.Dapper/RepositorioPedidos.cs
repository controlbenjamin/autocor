using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Proxys;
using AutocorApi.Repositorios.Dapper.Query;
using AutocorApi.Repositorios.Dapper.Query.Utils;
using AutocorApi.Repositorios.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioPedidos : RepositorioBase, IRepositorioPedidos
    {
        #region Queries

        private const string _QueryInsertPedido =
            @"INSERT INTO PEDIDOS
                           (COD_CLIENTE
                           ,FECHA
                           ,ID_ESTADO
                           ,FECHA_ENVIO
                           ,PRECIO_TOTAL
                           ,OBSERVACIONES)

                     VALUES
                           (@CodigoCliente
                           ,@Fecha
                           ,@IdEstadoPedido
                           ,@FechaEnvio
                           ,@ImporteTotal
                           ,@Observaciones);
                     SELECT CAST(SCOPE_IDENTITY() AS INT)";

        private const string _QueryIdPedidosEntrePaginas =
          @"SELECT p.ID_PEDIDO
            FROM PEDIDOS p
            INNER JOIN CLIENTES c ON p.COD_CLIENTE = c.CODCLI ";

        #endregion Queries

        private IEnumerable<Producto> _productosEnPedido = Enumerable.Empty<Producto>();

        public RepositorioPedidos() : base()
        {
        }

        public RepositorioPedidos(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Insertar(ref Pedido pedido)
        {
            pedido = ConvertPedidoToPedidoProxy(pedido);
            pedido.Id = Connection.ExecuteScalar<int>(_QueryInsertPedido, pedido, transaction: Transaction);
        }
        
        public Pedido ObtenerPorId(int idPedido)
        {
            string query = QueryPedido.QUERY +
                          " WHERE p.ID_PEDIDO = @IdPedido ";

            var res = Connection.Query<QueryPedido>(query, new { IdPedido = idPedido }, transaction: Transaction);
            return MapearPedido(res);
        }

        public IEnumerable<Pedido> ObtenerPorCliente(int codigoCliente)
        {
            string query = QueryPedido.QUERY +
                         " WHERE p.COD_CLIENTE = @CodigoCliente ";

            var res = Connection.Query<QueryPedido>(query, new { CodigoCliente = codigoCliente }, transaction: Transaction);
            return MapearPedidos(res);
        }

        public IEnumerable<Pedido> Buscar(int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta, PageConfig page = null)
        {
            DynamicParameters parametros = new DynamicParameters();

            if (page != null)
            {
                var idsPedidos = this.ObtenerIdPedidosEntrePaginas(codigoCliente, idEstado, zona, fechaDesde, fechaHasta, page);
                return ObtenerPorIds(idsPedidos);
            }

            string query = QueryPedido.CrearSelectWhere(parametros, codigoCliente, idEstado, zona, fechaDesde, fechaHasta, page);

            var res = Connection.Query<QueryPedido>(query, param: parametros, transaction: Transaction);
            return MapearPedidos(res);
        }

        public int CountBuscar(int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            DynamicParameters parametros = new DynamicParameters();
            string query = QueryPedido.CrearSelectCount(parametros, codigoCliente, idEstado, zona, fechaDesde, fechaHasta);
            return Connection.ExecuteScalar<int>(query, param: parametros, transaction: Transaction);
        }

        public IEnumerable<Pedido> ObtenerPorIds(IEnumerable<int> ids)
        {
            DynamicParameters parameters = new DynamicParameters();
            string query = QueryPedido.CrearSelectWhere(parameters, ids);
            var res = Connection.Query<QueryPedido>(query, param: parameters, transaction: Transaction);
            return MapearPedidos(res);
        }

        // private methods
        private PedidoProxy ConvertPedidoToPedidoProxy(Pedido pedido)
        {
            if (pedido is PedidoProxy)
                return (PedidoProxy)pedido;

            pedido = new PedidoProxy
            {
                Id = pedido.Id,
                CodigoCliente = pedido.CodigoCliente,
                Fecha = pedido.Fecha,
                FechaEnvio = pedido.FechaEnvio,
                ImporteTotal = pedido.ImporteTotal,
                IdEstadoPedido = pedido.IdEstadoPedido,
                Observaciones = pedido.Observaciones,
                Detalles = pedido.Detalles,
                Cliente = pedido.Cliente,
                NumeroPedidoSistema = pedido.NumeroPedidoSistema,
                Estado = pedido.Estado
            };

            return (PedidoProxy)pedido;
        }

        private Pedido MapearPedido(IEnumerable<QueryPedido> queryPedido)
        {
            if (queryPedido == null || queryPedido.Count() == 0)
                return null;

            Pedido pedido = new PedidoProxy();
            List<DetallePedido> detalles = new List<DetallePedido>();

            bool mapearPedido = true;
            foreach (var qp in queryPedido)
            {
                if (mapearPedido)
                {
                    pedido = qp.GetPedido();    // viene con el estado
                    mapearPedido = false;
                }

                DetallePedido detalle = qp.GetDetallePedido();
                detalle.Producto = _productosEnPedido.FirstOrDefault(p => p.CodigoPieza == detalle.CodigoPieza);
                detalles.Add(detalle);
            }

            pedido.Detalles = detalles;

            return pedido;
        }

        private IEnumerable<Pedido> MapearPedidos(IEnumerable<QueryPedido> queryPedidos)
        {
            if (queryPedidos == null || queryPedidos.Count() == 0)
                return Enumerable.Empty<Pedido>();

            List<Pedido> pedidos = new List<Pedido>();
            _productosEnPedido = new RepositorioProductos().ObtenerProductos(queryPedidos.Select(q => q.Detalles_CodigoPieza).ToArray());

            // agrupar los resultados por id de pedido
            var grupoPedidos = queryPedidos.GroupBy(qp => qp.Id, (k, g) => new { IdPedido = k, Results = g.ToList() });

            foreach (var item in grupoPedidos)
            {
                pedidos.Add(MapearPedido(item.Results));
            }

            return pedidos;
        }

        private IEnumerable<int> ObtenerIdPedidosEntrePaginas(int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta, PageConfig pageConfig)
        {
            DynamicParameters parameters = new DynamicParameters();

            string query = _QueryIdPedidosEntrePaginas + QueryPedido.CrearWhere(parameters, codigoCliente, idEstado, zona, fechaDesde, fechaHasta)
                    + " ORDER BY p.FECHA DESC " + QueryPaginacion.CrearPaginacion(parameters, pageConfig);

            return Connection.Query<int>(query, param: parameters, transaction: Transaction);
        }

    }
}