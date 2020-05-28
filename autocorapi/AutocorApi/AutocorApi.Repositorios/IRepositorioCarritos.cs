using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioCarritos : IRepositorio
    {
        void Insertar(IEnumerable<ItemCarrito> itemsCarrito);

        void Insertar(ItemCarrito itemCarrito);

        IEnumerable<ItemCarrito> ObtenerItemsCarritoPorCliente(int codigoCliente);

        void VaciarCarrito(int codigoCliente);

        void EliminarItemCarrito(int codigoCliente,string codigoPieza);

        void Actualizar(ItemCarrito itemCarrito);

        void Actualizar(IEnumerable<ItemCarrito> itemsCarrito);

        ItemCarrito Buscar(int codigoCliente, string codigoPieza);
    }
}