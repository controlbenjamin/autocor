using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioUsuariosWeb
    {
        PagedResultDto<UsuarioWebDto> ObtenerUsuariosWeb(int pagina, int tamañoPagina);
        PagedResultDto<UsuarioWebDto> BuscarUsuariosWeb(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda);
        bool ActualizarEstado(string nroCliente, bool estado);
        bool NuevoUsuariosWeb(int codigoCliente, string email, string nombrePC);
        bool CheckEstado(string nroCliente);
        bool BorrarUsuarioWeb(string nroCliente);
        bool CheckExisteMail(string mail);

    }
}
