using System;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Base;
using AutocorApi.Servicios.Core.Pedidos;
using AutocorApi.Servicios.Dto.Pedidos;
using AutocorApi.Servicios.Exceptions;
using AutocorApi.Servicios.Validation;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation.Pedidos
{
    public class ServicioPedidos : IServicioPedidos
    {
        private IUoW uow;

        public ServicioPedidos(IUoW uow)
        {
            this.uow = uow;
        }

        public PedidoDto GuardarPedido(NuevoPedidoDto pedido)
        {
            // validación
            var validacion = ValidarNuevoPedido(pedido);

            if (validacion.IsNotValid)
            {
                throw new ServiceValidationException(validacion);
            }

            var nuevoPedido = Mapper.Map<NuevoPedidoDto, Pedido>(pedido);
            nuevoPedido.Fecha = DateTime.Now;

            // setear el precio del detalle (precio vigente)
            var precioProductos = uow.RepositorioProductos.ObtenerPreciosProductos(pedido.Detalles.Select(d => d.CodigoPieza).ToArray());

            foreach (var detalle in nuevoPedido.Detalles)
            {
                detalle.PrecioUnitario = precioProductos.Single(p => p.CodigoPieza == detalle.CodigoPieza).PrecioVigente;
            }

            // setear el total del pedido
            nuevoPedido.ImporteTotal = nuevoPedido.Detalles.Sum(d => d.SubTotal);

            // insertar pedido
            uow.RepositorioPedidos.Insertar(ref nuevoPedido);

            foreach (var detalle in nuevoPedido.Detalles)
            {
                detalle.IdPedido = nuevoPedido.Id; // setear el id del pedido insertado anteriormente

                // insertar detalle
                uow.RepositorioDetallesPedidos.Insertar(detalle);
            }

            uow.Commit();

            return Mapper.Map<Pedido, PedidoDto>(nuevoPedido);
        }

        private ServiceValidation ValidarNuevoPedido(NuevoPedidoDto nuevoPedido)
        {
            var res = new ServiceValidation();

            if (nuevoPedido.CodigoCliente <= 0)
            {
                res.AddError("Cliente requerido", "CodigoCliente");
            }
            else if (!uow.RepositorioClientes.VerificarExistencia(nuevoPedido.CodigoCliente))
            {
                res.AddError("Cliente inexistente", "CodigoCliente");
            }

            if (nuevoPedido.Detalles.Count() == 0)
                res.AddError("No contiene detalle", "Detalles");

            return res;
        }
    }
}