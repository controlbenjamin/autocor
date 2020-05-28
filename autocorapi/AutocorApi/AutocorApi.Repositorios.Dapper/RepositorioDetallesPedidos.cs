using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Proxys;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioDetallesPedidos : RepositorioBase, IRepositorioDetallesPedidos
    {
        public RepositorioDetallesPedidos() : base()
        {
        }

        public RepositorioDetallesPedidos(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Insertar(DetallePedido detallePedido)
        {
            string sql = @"INSERT INTO DETALLE_PEDIDO
                                   (ID_PEDIDO
                                   ,COD_PIEZA
                                   ,CANTIDAD
                                   ,PRECIO_UNITARIO)
                             VALUES
                                   (@IdPedido
                                   ,@CodigoPieza
                                   ,@Cantidad
                                   ,@PrecioUnitario)";

            Connection.Execute(sql, detallePedido, transaction: Transaction);
        }

    }
}