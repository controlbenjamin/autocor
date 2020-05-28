using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioUsuarios : IRepositorio
    {
        Usuario Buscar(int idUsuario);

        Usuario Buscar(string nombreUsuario);

        Usuario Buscar(string nombreUsuario, string password);

        int ObtenerZonaPorUsuario(int idUsuario, Rol? rol = null);

        Usuario BuscarPorCodigoCliente(int codigoCliente);
    }
}