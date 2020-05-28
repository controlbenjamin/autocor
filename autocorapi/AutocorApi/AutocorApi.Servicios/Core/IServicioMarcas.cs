using AutocorApi.Servicios.Dto;
using System.Collections.Generic;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioMarcas
    {
        IEnumerable<MarcaDto> ObtenerMarcas();

        IEnumerable<MarcaDto> ObtenerMarcas(int? codigoRubro);

        IEnumerable<MarcaDto> ObtenerMarcasPorRubro(int codigoRubro);

        MarcaDto BuscarPorCodigo(string codigoMarca);
    }
}