using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Proxys;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioCarritos : RepositorioBase, IRepositorioCarritos
    {
        private static readonly string _QueryItemCarrito;

        static RepositorioCarritos()
        {
            _QueryItemCarrito = @"SELECT CODPIEZA as CodigoPieza
                                        ,CANTIDAD as Cantidad
                                        ,CODCLI as CodigoCliente
                                        ,FECHA as Fecha
                                   FROM CARRITOS ";
        }

        public RepositorioCarritos() : base()
        {
        }

        public RepositorioCarritos(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Actualizar(ItemCarrito itemCarrito)
        {
            string sql = @"UPDATE CARRITOS                     
                           SET CANTIDAD = @Cantidad
                           WHERE CODCLI = @CodigoCliente AND CODPIEZA = @CodigoPieza ";
            Connection.Execute(sql, itemCarrito, transaction: Transaction);
        }

        public void Actualizar(IEnumerable<ItemCarrito> itemsCarrito)
        {
            string sql = @"UPDATE CARRITOS                     
                           SET CANTIDAD = @Cantidad
                           WHERE CODCLI = @CodigoCliente AND CODPIEZA = @CodigoPieza ";
            Connection.Execute(sql, itemsCarrito, transaction: Transaction);
        }

        public ItemCarrito Buscar(int codigoCliente, string codigoPieza)
        {
            string query = _QueryItemCarrito +
                             " WHERE CODPIEZA = @CodigoPieza AND CODCLI = @CodigoCliente";

            return Connection.QueryFirstOrDefault<ItemCarritoProxy>(query,new {CodigoPieza = codigoPieza,CodigoCliente = codigoCliente},transaction:Transaction);
        }

        public void VaciarCarrito(int codigoCliente)
        {
            string sql = @"DELETE FROM CARRITOS
                           WHERE CODCLI = @CodigoCliente";
            Connection.Execute(sql, new { CodigoCliente = codigoCliente }, transaction: Transaction);
        }

        public void EliminarItemCarrito(int codigoCliente,string codigoPieza)
        {
            string sql = @"DELETE FROM CARRITOS
                           WHERE CODPIEZA = @CodigoPieza and CODCLI = @CodigoCliente";
            Connection.Execute(sql, new { CodigoCliente = codigoCliente,CodigoPieza = codigoPieza },transaction:Transaction);
        }

        public void Insertar(IEnumerable<ItemCarrito> itemsCarrito)
        {
            string sql = @"INSERT INTO CARRITOS 
                                (CODPIEZA, CANTIDAD, CODCLI, FECHA) 
                            VALUES(@CodigoPieza, @Cantidad, @CodigoCliente, @Fecha)";
            Connection.Execute(sql, itemsCarrito, transaction: Transaction);
        }

        public IEnumerable<ItemCarrito> ObtenerItemsCarritoPorCliente(int codigoCliente)
        {
            string query = _QueryItemCarrito + 
                              " WHERE CODCLI = @CodigoCliente"; 

            return Connection.Query<ItemCarritoProxy>(query, new { CodigoCliente = codigoCliente }, transaction: Transaction);
        }

        public void Insertar(ItemCarrito itemCarrito)
        {
            string sql = @"INSERT INTO CARRITOS 
                                (CODPIEZA, CANTIDAD, CODCLI, FECHA) 
                            VALUES(@CodigoPieza, @Cantidad, @CodigoCliente, @Fecha)";

            Connection.Execute(sql, itemCarrito, transaction: Transaction);
        }
    }
}