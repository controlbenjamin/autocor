using System;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioAutenticacion
    {
        UsuarioDto ValidarUsuario(string usuario, string password);

        void RegistrarInicioSesion(InicioSesionDto inicioSesion);

        ClienteAPIDto ObtenerClienteAPI(Guid id);

        UsuarioDto ValidarUsuarioWeb(string nombreUsuario, string password);
    }
}