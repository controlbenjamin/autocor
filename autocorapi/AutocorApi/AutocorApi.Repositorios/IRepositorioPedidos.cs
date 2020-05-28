using System;
using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Utils;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioPedidos : IRepositorio
    {
        Pedido ObtenerPorId(int idPedido);

        void Insertar(ref Pedido pedido);

        IEnumerable<Pedido> Buscar(int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta, PageConfig page = null);

        IEnumerable<Pedido> ObtenerPorIds(IEnumerable<int> ids);

        int CountBuscar(int? codigoCliente, int? idEstado, int? zona, DateTime? fechaDesde, DateTime? fechaHasta);
    }
}