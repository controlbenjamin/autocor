using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioToken
    {
        RefreshTokenDto ObtenerTokenPorId(string refreshToken);

        void EliminarToken(string idToken);

        void EliminarToken(string idToken, int idUsuario);

        void RegistrarToken(RefreshTokenDto refreshToken);

        string HashGuid(string token);
    }
}