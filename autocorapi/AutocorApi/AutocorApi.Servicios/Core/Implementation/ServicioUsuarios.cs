using AutocorApi.Repositorios;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private IRepositorioUsuarios repositorioUsuarios;

        public ServicioUsuarios(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        public int ObtenerZonaUsuarioViajante(int idUsuario)
        {
            return repositorioUsuarios.ObtenerZonaPorUsuario(idUsuario, rol: Entidades.Rol.VIAJANTE);
        }
    }
}