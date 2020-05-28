using System.Collections.Generic;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Productos;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioMiniCatalogo
    {
        IEnumerable<MarcaDto> ObtenerMarcas();

        IEnumerable<TipoAutoMinDto> ObtenerTiposAuto();

        IEnumerable<RubroMinDto> ObtenerRubros();

        IEnumerable<ProductoMinDto> ObtenerProductos(int? codigoRubro, string codigoMarca, int? codigoTipoAuto);
    }
}