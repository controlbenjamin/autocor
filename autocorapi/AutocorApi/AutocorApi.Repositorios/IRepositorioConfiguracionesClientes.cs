using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioConfiguracionesClientes : IRepositorio
    {
        ConfiguracionCliente BuscarPorCliente(int codigoCliente);

        void Insertar(ConfiguracionCliente configuracion);

        void Actualizar(ConfiguracionCliente configuracion);
    }
}