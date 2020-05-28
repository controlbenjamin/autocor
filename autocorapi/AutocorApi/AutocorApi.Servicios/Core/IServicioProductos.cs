using System.Collections.Generic;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Productos;
using AutocorApi.Servicios.Dto.Utils;

namespace AutocorApi.Servicios.Core
{
    public interface IServicioProductos
    {
        ProductoDto BuscarPorCodigo(string codigoPieza);

        StockDto ObtenerStockActual(string codigoPieza);

        IEnumerable<StockDto> ObtenerStockActual(string[] codigosPiezas);

        PagedResultDto<ProductoDto> ObtenerProductos(string descripcion, int pagina = 1, int tamanoPagina = 50);

        PagedResultDto<ProductoDto> ObtenerProductos(string descripcion, int? codigoRubro, string codigoMarca, int? codigoTipoAuto, int pagina = 1, int tamanoPagina = 50);

        IEnumerable<ProductoDto> ObtenerProductosEnOferta(int codigoRubro);

        IEnumerable<ProductoDto> ObtenerProductosIncorporados(int codigoRubro);

        IEnumerable<ProductoDto> ObtenerProductosEquivalentes(string codigoPieza);

        IEnumerable<ProductoDto> ObtenerProductosEquivalentes(string codigoPieza, bool incluirOrigen);

        IEnumerable<ParametroProductoDto> ObtenerParametros(string codigoPieza);

        IEnumerable<ProductoDto> ObtenerRandoms(int cantidad = 10);

        IEnumerable<ProductoDto> ObtenerMasPedidosEnMes(int cantidad = 10, int? zona = null);
    }
}