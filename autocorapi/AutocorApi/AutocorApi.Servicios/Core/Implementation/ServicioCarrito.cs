using System;
using System.Collections.Generic;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Base;
using AutocorApi.Servicios.Dto.Carrito;
using AutocorApi.Servicios.Exceptions;
using AutocorApi.Servicios.Validation;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioCarrito : IServicioCarrito
    {
        private IUoW uow;

        public ServicioCarrito(IUoW uow)
        {
            this.uow = uow;
        }

        public IEnumerable<ItemCarritoDto> ObtenerCarritoPorCliente(int codigoCliente)
        {
            var items = uow.RepositorioCarritos.ObtenerItemsCarritoPorCliente(codigoCliente);
            return Mapper.Map<IEnumerable<ItemCarrito>, IEnumerable<ItemCarritoDto>>(items);
        }

        public void GuardarItemsCarrito(int codigoCliente, IEnumerable<EditItemCarritoDto> itemsCarritoDto)
        {
            if (itemsCarritoDto == null || itemsCarritoDto.Count() == 0)
                return;

            var validacion = ValidacionCarrito(codigoCliente, itemsCarritoDto);

            if (validacion.IsNotValid)
                throw new ServiceValidationException(validacion);

            // aseguro que todos los items sean del mismo cliente
            //foreach (var item in itemsCarritoDto) { item.CodigoCliente = codigoCliente; }

            // mapeo a entidad
            var itemsCarrito = Mapper.Map<IEnumerable<EditItemCarritoDto>, IEnumerable<ItemCarrito>>(itemsCarritoDto);

            foreach (var item in itemsCarrito)
            {
                item.CodigoCliente = codigoCliente;
                item.Fecha = DateTime.Now;
            }

            string[] piezasEnCarrito = uow.RepositorioCarritos.ObtenerItemsCarritoPorCliente(codigoCliente).Select(i => i.CodigoPieza).ToArray();

            var itemsActualizar = itemsCarrito.Where(i => piezasEnCarrito.Contains(i.CodigoPieza));
            var itemsNuevos = itemsCarrito.Where(i => !piezasEnCarrito.Contains(i.CodigoPieza));

            if (itemsNuevos.Count() > 0)
                uow.RepositorioCarritos.Insertar(itemsNuevos);

            if (itemsActualizar.Count() > 0)
                uow.RepositorioCarritos.Actualizar(itemsActualizar);

            uow.Commit();
        }

        public void GuardarItemCarrito(EditItemCarritoDto itemCarrito, bool acumularCantidad = false)
        {
            var validacion = ValidacionCarrito(itemCarrito);

            if (validacion.IsNotValid)
                throw new ServiceValidationException(validacion);

            // verificar si existe en el carrito
            var item = uow.RepositorioCarritos.Buscar(itemCarrito.CodigoCliente, itemCarrito.CodigoPieza);

            ItemCarrito nuevoItem = Mapper.Map<EditItemCarritoDto, ItemCarrito>(itemCarrito);

            if (item == null)
            {
                // el item no está
                nuevoItem.Fecha = DateTime.Now;
                uow.RepositorioCarritos.Insertar(nuevoItem);
            }
            else
            {
                if (acumularCantidad)
                {
                    nuevoItem.Cantidad += item.Cantidad;
                }

                uow.RepositorioCarritos.Actualizar(nuevoItem);
            }

            uow.Commit();
        }

        public void VaciarCarrito(int codigoCliente)
        {
            uow.RepositorioCarritos.VaciarCarrito(codigoCliente);
            uow.Commit();
        }

        public void EliminarItemCarrito(int codigoCliente, string codigoPieza)
        {
            uow.RepositorioCarritos.EliminarItemCarrito(codigoCliente, codigoPieza);
            uow.Commit();
        }

        private ServiceValidation ValidacionCarrito(EditItemCarritoDto itemCarrito)
        {
            var validacion = new ServiceValidation();

            if (!uow.RepositorioClientes.VerificarExistencia(itemCarrito.CodigoCliente))
                validacion.AddError("Cliente inexistente", "CodigoCliente");

            if (!uow.RepositorioProductos.VerificarExistencia(itemCarrito.CodigoPieza))
                validacion.AddError("Producto inexistente", "CodigoPieza");

            return validacion;
        }

        private ServiceValidation ValidacionCarrito(int codigoCliente, IEnumerable<EditItemCarritoDto> itemsCarrito)
        {
            var validacion = new ServiceValidation();

            if (!uow.RepositorioClientes.VerificarExistencia(codigoCliente))
                validacion.AddError("Cliente inexistente", "CodigoCliente");

            var noExisten = uow.RepositorioProductos.VerificarExistencia(itemsCarrito.Select(x => x.CodigoPieza).ToArray());

            if (noExisten.Count() > 0)
                validacion.AddError("Productos inexistentes", "");

            return validacion;
        }
    }
}