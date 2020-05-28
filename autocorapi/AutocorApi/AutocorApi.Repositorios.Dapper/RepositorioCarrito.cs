using AutocorApi.Repositorios.Dapper.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
   public class RepositorioCarrito : RepositorioBase,IRepositorioCarrito
    {
        public RepositorioCarrito():base()
        {
        }

        public RepositorioCarrito(IDbTransaction transaction) : base(transaction)
        {
        }

        public void ActualizarCantidad(DetalleCarrito dc)
        {
        }

        public int agregarProductoCarrito(DetalleCarrito itemCarrito, int cantidad)
        {
            int resultado = 0;
            string sql = @"INSERT INTO CARRITOS(CODPIEZA,CANTIDAD)VALUES(@CodigoPieza,@Cantidad)";
            //resultado = Connection.Execute(sql, itemCarrito);
            //return resultado;
            var item = _BuscarPorCodigoArticulo(itemCarrito.CodigoPieza);
            if (item == null)
            {
                resultado = Connection.Execute(sql, itemCarrito);
            }
            else
            {
                itemCarrito.Cantidad += item.Cantidad;
            }
            return resultado;
        }

        public IEnumerable<DetalleCarrito> ObtenerCarrito(bool validarExiste)
        {
            throw new NotImplementedException();
        }

        public int EliminarProducto(string codigoPieza)
        {
            int resultado;
            string sql = "DELETE FROM CARRITOS WHERE CODPIEZA = @CodigoPieza";
            try
            {
                resultado = Connection.Execute(sql, new { CodigoPieza = codigoPieza });
            }
            catch (Exception ex)
            {

                throw new Exception("Error al borrar el producto del carrito", ex);
            }
            return resultado;
        }

        public int VaciarCarrito()
        {
            int resultado;
            string sql = "DELETE FROM CARRITOS";
            try
            {
                resultado = Connection.Execute(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al vaciar carrito",ex);
            }
            return resultado;
        }

        public DetalleCarrito _BuscarPorCodigoArticulo(string codigoPieza)
        {
            DetalleCarrito itemCarrito = null;
            string query = @"";
            //itemCarrito = connection.QueryFirstOrDefault<ItemCarrito>(query, new { CodigoArticulo = codigoArticulo });
            itemCarrito = Connection.QueryFirstOrDefault<DetalleCarrito>(query, new { CodigoPieza = codigoPieza });
            return itemCarrito;
        }

        private int _ActualizarItem(DetalleCarrito itemCarrito)
        {
            int resultado;
            string sql = "UPDATE CARRITOS " +
                            @"SET art_codigo=@CodigoArticulo, 
                                  car_cantidad = @Cantidad, 
                                  car_precio_unitario = @PrecioUnitario
                            WHERE art_codigo = @CodigoArticulo";

            resultado = Connection.Execute(sql, itemCarrito);

            return resultado;
        }

    }
}

