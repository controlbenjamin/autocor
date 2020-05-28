using System;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Utils;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioIniciosSesion
    {
        PagedResultDto<InicioSesionDto> Buscar(int? codigoCliente = null,
             string tipoCatalogo = null,
            DateTime? fechaDesde = null, DateTime? fechaHasta = null,
           int pagina = 1, int tamanoPagina = 50);

        PagedResultDto<InicioSesionDto> ObtenerCambiosUsuariosEscritorio(int? codigoCliente = null,
            DateTime? fechaDesde = null, DateTime? fechaHasta = null,
           int pagina = 1, int tamanoPagina = 50);
    }
}