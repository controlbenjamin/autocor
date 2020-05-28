using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioEstadosPedidos : RepositorioBase, IRepositorioEstadosPedidos
    {
        public RepositorioEstadosPedidos() : base()
        {
        }

        public RepositorioEstadosPedidos(IDbTransaction transaction) : base(transaction)
        {
        }

        public IEnumerable<EstadoPedido> ObtenerEstadosPedidos()
        {
            string query = @"SELECT ep.ID_ESTADO as Id
                                   ,ep.DESCRIPCION as Descripcion
                             FROM ESTADO_PEDIDO ep
                             ORDER BY ep.DESCRIPCION ASC";

            return Connection.Query<EstadoPedido>(query, transaction: Transaction);
        }

        public EstadoPedido ObtenerPorId(int idEstadoPedido)
        {
            string query = @"SELECT ep.ID_ESTADO as Id
                                   ,ep.DESCRIPCION as Descripcion
                             FROM ESTADO_PEDIDO ep
                             WHERE ep.ID_ESTADO = @IdEstadoPedido";

            return Connection.QuerySingleOrDefault<EstadoPedido>(query,new { IdEstadoPedido = idEstadoPedido }, transaction: Transaction);
        }
    }
}