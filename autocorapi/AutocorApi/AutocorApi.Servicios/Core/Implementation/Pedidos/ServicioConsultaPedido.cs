using System;
using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Utils;
using AutocorApi.Servicios.Core.Pedidos;
using AutocorApi.Servicios.Dto.Pedidos;
using AutocorApi.Servicios.Dto.Utils;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation.Pedidos
{
    public class ServicioConsultaPedidos : IServicioConsultaPedidos
    {
        private IRepositorioPedidos _repoPedidos;

        public ServicioConsultaPedidos(IRepositorioPedidos repoPedidos)
        {
            this._repoPedidos = repoPedidos;
        }

        public PagedResultDto<PedidoDto> Buscar(int? codigoCliente = null, int? idEstado = null, int? zonaCliente = null,  DateTime? fechaDesde = null, DateTime? fechaHasta = null, int pagina = 1, int tamanoPagina = 50)
        {
            var pedidos = _repoPedidos.Buscar(codigoCliente, idEstado, zonaCliente, fechaDesde, fechaHasta, PageConfig.Create(pagina, tamanoPagina));
            int totalPedidos = _repoPedidos.CountBuscar(codigoCliente, idEstado, zonaCliente, fechaDesde, fechaHasta);

            var pedidosDto = Mapper.Map<IEnumerable<Pedido>, IEnumerable<PedidoDto>>(pedidos);

            return new PagedResultDto<PedidoDto>(pedidosDto, pagina, tamanoPagina, totalPedidos);
        }

        public PedidoDto BuscarPorId(int idPedido)
        {
            Pedido pedido = _repoPedidos.ObtenerPorId(idPedido);
            return Mapper.Map<Pedido, PedidoDto>(pedido);
        }
    }
}