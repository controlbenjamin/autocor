using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioInstalaciones
    {
        PagedResultDto<InstalacionDto> ObtenerInstalaciones(int pagina, int tamañoPagina);
        PagedResultDto<InstalacionDto> BuscarInstalaciones(int pagina, int tamañoPagina,string criterioBusqueda, DateTime? fechaInicialdeBusqueda);
        bool ActualizarEstado(string instalacion, bool estado);
        InstalacionDto NuevaInstalacion(int codigoCliente,string email, string nombrePC);
    }
}
