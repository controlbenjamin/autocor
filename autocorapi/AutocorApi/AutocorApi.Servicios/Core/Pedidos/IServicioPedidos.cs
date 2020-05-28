using AutocorApi.Entidades;
using AutocorApi.Servicios.Dto.Pedidos;

namespace AutocorApi.Servicios.Core.Pedidos
{
    public interface IServicioPedidos
    {
        /// <summary>
        /// Guardar un nuevo pedido
        /// </summary>
        /// <param name="pedido">El nuevo pedido a guardar</param>
        /// <returns>El pedido generado</returns>
        PedidoDto GuardarPedido(NuevoPedidoDto pedido);
    }
}