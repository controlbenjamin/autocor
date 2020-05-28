using System.Collections.Generic;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioTiposAuto
    {
        IEnumerable<TipoAutoDto> ObtenerTiposAuto();

        IEnumerable<TipoAutoDto> ObtenerTiposAutoPorMarca(string codigoMarca);

        TipoAutoDto BuscarPorCodigo(int codigoTipoAuto);

        IEnumerable<TipoAutoMinDto> ObtenerTiposAutoMin(string codigoMarca = null);

        TipoAutoMinDto BuscarMinPorCodigo(int codigoTipoAuto);
    }
}