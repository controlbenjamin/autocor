using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Clientes;

namespace AutocorApi.Servicios.Email
{
    public interface IServicioEmail
    {
        bool EnviarEmailConsulta(ConsultaDto consulta);

        bool EnviarEmailInicioSesionCatalogoEscritorio(ClienteDto cliente, InicioSesionDto inicioSesion, bool notificiarAutocor);

        bool EnviarEmailRegistro(RegistroDto registro);

        bool EnviarEmailConsultaSitioWeb(ConsultaWebDto consulta);

        bool EnviarEmailAltaClienteWeb(AltaClienteWebDto altaCliente);
        bool EnviarEmailRestaurarClave(string emailReset);
    }
}