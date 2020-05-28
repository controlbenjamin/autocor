using System;
using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioActualizaciones : IRepositorio
    {
        IEnumerable<Actualizacion> ObtenerUltimasActualizaciones(int cantidad);

        Actualizacion ObtenerUltimaActualizacion();

        Actualizacion BuscarPorId(int idActualizacion);

        int Count(DateTime? desde, DateTime? hasta);

        int Count();

        void Insertar(Actualizacion actualizacion);
    }
}