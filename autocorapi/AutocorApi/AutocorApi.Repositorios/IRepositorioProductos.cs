using AutocorApi.Entidades;
using AutocorApi.Repositorios.Utils;
using System;
using System.Collections.Generic;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioProductos : IRepositorio
    {
        Producto BuscarPorCodigo(string codigoPieza, bool incluirParametros = true);

        IEnumerable<Producto> ObtenerProductos(string[] codigosPiezas, bool incluirParametros = false, PageConfig config = null);

        int CountObtenerProductos(string[] codigosPiezas);

        IEnumerable<ParametroProducto> ObtenerParametrosProducto(string codigoPieza);

        IEnumerable<ParametroProducto> ObtenerParametrosProductos(string[] codigosPieza);

        IEnumerable<Producto> ObtenerProductos(string descripcion, int? codigoRubro, string codigoMarca, int? codigoTipoAuto, bool enOferta = false, bool incluirParametros = false, PageConfig config = null);

        int CountObtenerProductos(string descripcion, int? codigoRubro, string codigoMarca, int? codigoTipoAuto, bool enOferta = false);

        IEnumerable<Producto> ObtenerProductosEquivalentes(string codigoPieza, bool incluirParametros = false);

        bool VerificarExistenciaDeProductosEquivalentes(string codigoPieza);

        IEnumerable<string> VerificarExistenciaDeGrupoProductosEquivalentes(string[] codigosPiezas);

        IEnumerable<Producto> ObtenerProductosEnOferta(bool incluirParametros = false);

        IEnumerable<Producto> ObtenerProductosEnOfertaPorRubro(int codigoRubro, bool incluirParametros = false);

        IEnumerable<Producto> ObtenerProductosIncorporados(bool incluirParametros = false);

        IEnumerable<Producto> ObtenerProductosIncorporadosPorRubro(int codigoRubro, bool incluirParametros = false);

        int ObtenerStockActual(string codigoPieza);

        /// <summary>
        /// Verifica la existencia de productos
        /// </summary>
        /// <returns>Códigos de productos inexistentes</returns>
        IEnumerable<string> VerificarExistencia(string[] codigosPiezas);

        bool VerificarExistencia(string codigoPieza);

        IEnumerable<Producto> ObtenerRandoms(int cantidad);

        IEnumerable<PrecioProducto> ObtenerPreciosProductos(string[] codigosPiezas);

        IEnumerable<ProductoBase> ObtenerProductosBase(int? codigoRubro, string codigoMarca, int? codigoTipoAuto);

        IEnumerable<Producto> ObtenerProductosMasPedidos(DateTime desde, DateTime hasta, int cantidad = 10, int? zona = null);
    }
}