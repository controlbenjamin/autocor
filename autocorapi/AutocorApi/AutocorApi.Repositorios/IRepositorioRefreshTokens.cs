using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioRefreshTokens : IRepositorio
    {
        RefreshToken ObtenerPorId(string id);

        void Eliminar(string id, int? idUsuario = null);

        void Insertar(RefreshToken refreshToken);
    }
}