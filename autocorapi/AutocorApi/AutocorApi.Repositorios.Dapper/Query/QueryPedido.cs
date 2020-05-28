using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Proxys;
using AutocorApi.Repositorios.Dapper.Query.Utils;
using AutocorApi.Repositorios.Utils;
using Dapper;
using System;
using System.Collections.Generic;

namespace AutocorApi.Repositorios.Dapper.Query
{
    /// <summary>
    /// Clase de query que contiene los datos de pedido, estado y detalle del pedido en cada item
    /// </summary>
    internal class QueryPedido
    {
        public const string QUERY = @"SELECT p.ID_PEDIDO as Id
                              ,p.COD_CLIENTE as CodigoCliente
                              ,p.FECHA as Fecha
                              ,p.FECHA_ENVIO as FechaEnvio
                              ,p.PRECIO_TOTAL as ImporteTotal
                              ,p.OBSERVACIONES as Observaciones
                              ,p.ID_ESTADO as IdEstadoPedido
                              ,p.NRO_PEDI_SISTEMA as NumeroPedidoSistema

                              ,ep.ID_ESTADO as Estado_Id
                              ,ep.DESCRIPCION as Estado_Descripcion

                              ,dp.ID_PEDIDO as Detalles_IdPedido
                              ,dp.COD_PIEZA as Detalles_CodigoPieza
                              ,dp.CANTIDAD as Detalles_Cantidad
                              ,dp.PRECIO_UNITARIO as Detalles_PrecioUnitario

                              ,s.DESCRIP as Detalles_Descripcion

                              ,c.NOMBRE as Cliente_Nombre
                              ,c.CUIT as Cliente_CUIT
                              ,c.ZON_CODIGO as Cliente_Zona

                          FROM DETALLE_PEDIDO dp
                          INNER JOIN PEDIDOS p ON dp.ID_PEDIDO = p.ID_PEDIDO
                          INNER JOIN ESTADO_PEDIDO ep ON p.ID_ESTADO = ep.ID_ESTADO
                          INNER JOIN STOCK s ON dp.COD_PIEZA = s.CODPIEZA
                          INNER JOIN CLIENTES c ON p.COD_CLIENTE = c.CODCLI ";

        // pedido
        public int Id { get; set; }

        public int CodigoCliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public decimal ImporteTotal { get; set; }
        public string Observaciones { get; set; }
        public int IdEstadoPedido { get; set; }
        public int NumeroPedidoSistema { get; set; }

        // cliente
        public string Cliente_Nombre { get; set; }
        public string Cliente_CUIT { get; set; }
        public int Cliente_Zona { get; set; }

        // estado
        public int Estado_Id { get; set; }
        public string Estado_Descripcion { get; set; }

        // detalle pedido
        public int Detalles_IdPedido { get; set; }

        public string Detalles_CodigoPieza { get; set; }
        public int Detalles_Cantidad { get; set; }
        public decimal Detalles_PrecioUnitario { get; set; }
        public string Detalles_Descripcion { get; set; }

        public Pedido GetPedido()
        {
            return new Pedido
            {
                Id = Id,
                CodigoCliente = CodigoCliente,
                Fecha = Fecha,
                FechaEnvio = FechaEnvio,
                IdEstadoPedido = IdEstadoPedido,
                ImporteTotal = ImporteTotal,
                Observaciones = Observaciones,
                NumeroPedidoSistema = NumeroPedidoSistema,
                Estado = new EstadoPedido
                {
                    Id = Estado_Id,
                    Descripcion = Estado_Descripcion
                },
                Cliente = new ClienteBase
                {
                    Codigo = CodigoCliente,
                    CodigoZona = Cliente_Zona,
                    RazonSocial = Cliente_Nombre,
                    CUIT = Cliente_CUIT
                }
            };
        }

        public DetallePedido GetDetallePedido()
        {
            return new DetallePedidoProxy
            {
                IdPedido = Detalles_IdPedido,
                Cantidad = Detalles_Cantidad,
                CodigoPieza = Detalles_CodigoPieza,
                PrecioUnitario = Detalles_PrecioUnitario,
                Descripcion = Detalles_Descripcion,
                Producto = null
            };
        }

        public static string CrearSelectWhere(DynamicParameters parametros, int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta, PageConfig pagination)
        {
            if (parametros == null)
            {
                throw new ArgumentNullException(nameof(parametros));
            }

            string query = QUERY + CrearWhere(parametros, codigoCliente, idEstado, zona, fechaDesde, fechaHasta);

            query += " ORDER BY p.FECHA DESC ";

            // paginación
            if (pagination != null)
            {
                query += QueryPaginacion.CrearPaginacion(parametros, pagination);
            }

            return query;
        }

        public static string CrearSelectWhere(DynamicParameters parameters, IEnumerable<int> idsPedidos)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            string query = QUERY + " WHERE p.ID_PEDIDO IN @IdsPedidos";
            parameters.Add("IdsPedidos", idsPedidos);

            query += " ORDER BY p.FECHA DESC ";

            return query;
        }
            
        public static string CrearSelectCount(DynamicParameters parametros, int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            string query =
                @"SELECT COUNT(*)
                 FROM PEDIDOS p
                 INNER JOIN CLIENTES c ON p.COD_CLIENTE = c.CODCLI ";

            return query + CrearWhere(parametros, codigoCliente, idEstado, zona, fechaDesde, fechaHasta);
        }

        public static string CrearWhere(DynamicParameters parametros, int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            string where = " WHERE 1 = 1 ";

            if (codigoCliente.HasValue)
            {
                where += " AND p.COD_CLIENTE = @CodigoCliente ";
                parametros.Add("CodigoCliente", codigoCliente.Value);
            }

            if (idEstado.HasValue)
            {
                where += " AND p.ID_ESTADO = @IdEstado ";
                parametros.Add("ID_ESTADO", idEstado.Value);
            }

            if (zona.HasValue)
            {
                where += " AND c.ZON_CODIGO = @Zona ";
                parametros.Add("Zona", zona.Value);
            }

            if (fechaDesde.HasValue)
            {
                where += " AND p.FECHA >= @FechaDesde ";
                parametros.Add("FechaDesde", fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                where += " AND p.FECHA <= @FechaHasta ";
                parametros.Add("FechaHasta", fechaHasta.Value);
            }

            return where;
        }
    }
}