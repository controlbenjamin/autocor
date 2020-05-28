using AutocorApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioCarrito
    {
        IEnumerable<DetalleCarrito> ObtenerCarrito(bool validarExiste);

        int VaciarCarrito();

        int EliminarProducto(string codigoPieza);

        int agregarProductoCarrito(DetalleCarrito itemCarrito, int cantidad);

        void ActualizarCantidad(DetalleCarrito dc);

    }
}
