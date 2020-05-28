using System;
using System.Collections.Generic;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Utils;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Productos;
using AutocorApi.Servicios.Dto.Utils;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioProductos : IServicioProductos
    {
        private const int MAX_RANDOM_PRODUCTOS = 100;

        private IRepositorioProductos repositorioProductos;

        public ServicioProductos(IRepositorioProductos repoProductos)
        {
            this.repositorioProductos = repoProductos;
        }

        public ProductoDto BuscarPorCodigo(string codigoPieza)
        {
            var producto = repositorioProductos.BuscarPorCodigo(codigoPieza);
            return Mapper.Map<ProductoDto>(producto);
        }

        public IEnumerable<ProductoDto> ObtenerMasPedidosEnMes(int cantidad = 10, int? zona = null)
        {
            var hasta = DateTime.Now.Date;
            var desde = hasta.AddMonths(-1);

            var productos = repositorioProductos.ObtenerProductosMasPedidos(desde, hasta, cantidad: cantidad, zona: zona);
            return Mapper.Map<IEnumerable<Producto>, IEnumerable<ProductoDto>>(productos);
        }

        public IEnumerable<ParametroProductoDto> ObtenerParametros(string codigoPieza)
        {
            var parametros = repositorioProductos.ObtenerParametrosProducto(codigoPieza);
            return Mapper.Map<IEnumerable<ParametroProducto>, IEnumerable<ParametroProductoDto>>(parametros);
        }

        public PagedResultDto<ProductoDto> ObtenerProductos(string descripcion, int pagina = 1, int tamanoPagina = 50)
        {
            return this.ObtenerProductos(descripcion, null, null, null, pagina, tamanoPagina);
        }

        public PagedResultDto<ProductoDto> ObtenerProductos(string descripcion, int? codigoRubro, string codigoMarca, int? codigoTipoAuto, int pagina = 1, int tamanoPagina = 50)
        {
            var config = PageConfig.Create(pagina, tamanoPagina);
            var productos = repositorioProductos.ObtenerProductos(descripcion, codigoRubro, codigoMarca, codigoTipoAuto, enOferta: false, incluirParametros: false, config: config);

            // obtener parámetros
            string[] codigosPiezas = productos.Select(x => x.CodigoPieza).ToArray();
            var parametros = repositorioProductos.ObtenerParametrosProductos(codigosPiezas);

            // verificador de equivalencia
            var productosConEquivalencia = repositorioProductos.VerificarExistenciaDeGrupoProductosEquivalentes(codigosPiezas);

            foreach (var producto in productos)
            {
                producto.Parametros = parametros.Where(x => producto.CodigoPieza == x.CodigoPieza);
                producto.TieneEquivalencias = productosConEquivalencia.Contains(producto.CodigoPieza);
            }

            int totalProductos = repositorioProductos.CountObtenerProductos(descripcion, codigoRubro, codigoMarca, codigoTipoAuto, enOferta: false);

            var productosDto = Mapper.Map<IEnumerable<ProductoDto>>(productos);
            return new PagedResultDto<ProductoDto>(productosDto, pagina, tamanoPagina, totalProductos);
        }

        public IEnumerable<ProductoDto> ObtenerProductosEnOferta(int codigoRubro)
        {
            var productos = repositorioProductos.ObtenerProductosEnOfertaPorRubro(codigoRubro, incluirParametros: false);
            return Mapper.Map<IEnumerable<Producto>, IEnumerable<ProductoDto>>(productos);
        }

        public IEnumerable<ProductoDto> ObtenerProductosEquivalentes(string codigoPieza)
        {
            return this.ObtenerProductosEquivalentes(codigoPieza, false);
        }

        public IEnumerable<ProductoDto> ObtenerProductosEquivalentes(string codigoPieza, bool incluirOrigen)
        {
            var productos = repositorioProductos.ObtenerProductosEquivalentes(codigoPieza, incluirParametros: false);

            if (incluirOrigen)
            {
                Producto productoOrigen = repositorioProductos.BuscarPorCodigo(codigoPieza);
                var listaProductos = productos.ToList();
                listaProductos.Insert(0, productoOrigen);
                productos = listaProductos;
            }

            return Mapper.Map<IEnumerable<Producto>, IEnumerable<ProductoDto>>(productos);
        }

        public IEnumerable<ProductoDto> ObtenerProductosIncorporados(int codigoRubro)
        {
            var productos = repositorioProductos.ObtenerProductosIncorporadosPorRubro(codigoRubro);
            return Mapper.Map<IEnumerable<Producto>, IEnumerable<ProductoDto>>(productos);
        }

        public IEnumerable<ProductoDto> ObtenerRandoms(int cantidad = 10)
        {
            // limitar la cantidad de randoms
            cantidad = cantidad > MAX_RANDOM_PRODUCTOS ? MAX_RANDOM_PRODUCTOS : cantidad;

            var randoms = repositorioProductos.ObtenerRandoms(cantidad);
            return Mapper.Map<IEnumerable<Producto>, IEnumerable<ProductoDto>>(randoms);
        }

        public IEnumerable<StockDto> ObtenerStockActual(string[] codigosPiezas)
        {
            var productos = repositorioProductos.ObtenerProductos(codigosPiezas, false);
            return Mapper.Map<IEnumerable<Producto>, IEnumerable<StockDto>>(productos);
        }

        public StockDto ObtenerStockActual(string codigoPieza)
        {
            int stock = repositorioProductos.ObtenerStockActual(codigoPieza);
            var estado = Producto.GetEstadoStock(stock);
            return new StockDto { CodigoPieza = codigoPieza, EstadoStock = estado.ToString(), StockReal = stock };
        }
    }
}