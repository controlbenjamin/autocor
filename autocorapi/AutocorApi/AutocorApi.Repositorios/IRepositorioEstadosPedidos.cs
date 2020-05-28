using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioEstadosPedidos : IRepositorio
    {
        EstadoPedido ObtenerPorId(int idEstadoPedido);

        IEnumerable<EstadoPedido> ObtenerEstadosPedidos();
    }
}