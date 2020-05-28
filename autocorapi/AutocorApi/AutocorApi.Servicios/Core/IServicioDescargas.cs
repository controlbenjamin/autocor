using System.Collections.Generic;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioDescargas
    {
        IEnumerable<DescargaDto> ObtenerDescargas();

        DescargaDto ObtenerPorId(int idDescarga);
    }
}