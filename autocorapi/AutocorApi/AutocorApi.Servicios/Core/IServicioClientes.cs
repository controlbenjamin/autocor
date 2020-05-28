using System.Collections.Generic;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Clientes;
using AutocorApi.Servicios.Dto.Utils;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioClientes
    {
        ClienteDto BuscarPorNumero(int numero);

        ClienteDto BuscarPorCuit(string cuit);

        ClienteDto BuscarPorUsuario(string nombreUsuario);

        void GuardarConfiguracion(ConfiguracionClienteDto configuracion);

        IEnumerable<ClienteDto> BuscarPorRazonSocial(string razonSocial, int? zona = null, int? gira = null);

        PagedResultDto<ClienteMinDto> ObtenerListado(int? zona = null, int pagina = 1, int tamanoPagina = 50);

        //void testing(); // test-borrar
    }
}