using System;
using AutocorApi.Servicios.Dto.Pedidos;
using AutocorApi.Servicios.Dto.Utils;

namespace AutocorApi.Servicios.Core.Pedidos
{
    public interface IServicioConsultaPedidos
    {
        PedidoDto BuscarPorId(int idPedido);

        PagedResultDto<PedidoDto> Buscar(int? codigoCliente = null, int? idEstado = null, int? zonaCliente = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, int pagina = 1, int tamanoPagina = 50);
    }
}