using System.Collections.Generic;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioActualizaciones
    {
        ActualizacionDto ObtenerUltimaActualizacion();

        IEnumerable<ActualizacionDto> ObtenerUltimasActualizaciones(int cantidad = 5);

        void NuevaActualizacion(ActualizacionDto actualizacion);
    }
}