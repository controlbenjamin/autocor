using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioDetallesPedidos : IRepositorio
    {
        void Insertar(DetallePedido detallePedido);
    }
}