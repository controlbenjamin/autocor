using System.Collections.Generic;
using AutocorApi.Servicios.Dto.Carrito;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioCarrito
    {
        IEnumerable<ItemCarritoDto> ObtenerCarritoPorCliente(int codigoCliente);

        void GuardarItemsCarrito(int codigoCliente, IEnumerable<EditItemCarritoDto> itemsCarrito);

        void GuardarItemCarrito(EditItemCarritoDto itemCarrito, bool acumularCantidad = false);

        void VaciarCarrito(int codigoCliente);

        void EliminarItemCarrito(int codigoCliente, string codigoPieza);
    }
}