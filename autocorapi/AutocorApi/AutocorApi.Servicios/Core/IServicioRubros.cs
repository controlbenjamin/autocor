using System.Collections.Generic;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioRubros
    {
        IEnumerable<RubroDto> ObtenerRubros();

        IEnumerable<RubroDto> ObtenerRubrosConIncorporaciones();

        IEnumerable<RubroDto> ObtenerRubrosConOfertas();

        RubroDto BuscarPorCodigo(int codigoRubro);

        RubroMinDto BuscarMinPorCodigo(int codigoRubro);

        IEnumerable<RubroMinDto> ObtenerRubrosMin();
    }
}