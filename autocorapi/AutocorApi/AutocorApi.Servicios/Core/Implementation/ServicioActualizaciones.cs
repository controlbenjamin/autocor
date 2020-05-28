using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioActualizaciones : IServicioActualizaciones
    {
        private IRepositorioActualizaciones repositorioActualizaciones;

        public ServicioActualizaciones(IRepositorioActualizaciones repositorioActualizaciones)
        {
            this.repositorioActualizaciones = repositorioActualizaciones;
        }

        public void NuevaActualizacion(ActualizacionDto actualizacion)
        {
            var nuevaActualizacion = Mapper.Map<ActualizacionDto, Actualizacion>(actualizacion);
            repositorioActualizaciones.Insertar(nuevaActualizacion);
        }

        public ActualizacionDto ObtenerUltimaActualizacion()
        {
            var ultimaActualizacion = repositorioActualizaciones.ObtenerUltimaActualizacion();
            return Mapper.Map<Actualizacion, ActualizacionDto>(ultimaActualizacion);
        }

        public IEnumerable<ActualizacionDto> ObtenerUltimasActualizaciones(int cantidad = 5)
        {
            var ultimasActualizaciones = repositorioActualizaciones.ObtenerUltimasActualizaciones(cantidad);
            return Mapper.Map<IEnumerable<Actualizacion>, IEnumerable<ActualizacionDto>>(ultimasActualizaciones);
        }
    }
}