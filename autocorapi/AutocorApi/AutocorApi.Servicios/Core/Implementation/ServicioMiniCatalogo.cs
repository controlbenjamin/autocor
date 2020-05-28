using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Productos;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioMiniCatalogo : IServicioMiniCatalogo
    {
        private IRepositorioProductos repositorioProductos;
        private IRepositorioRubros repositorioRubros;
        private IRepositorioTiposAuto repositorioTiposAuto;
        private IRepositorioMarcas repositorioMarcas;

        public ServicioMiniCatalogo(IRepositorioProductos repositorioProductos,
            IRepositorioRubros repositorioRubros, IRepositorioTiposAuto repositorioTiposAuto,
            IRepositorioMarcas repositorioMarcas)
        {
            this.repositorioProductos = repositorioProductos;
            this.repositorioRubros = repositorioRubros;
            this.repositorioTiposAuto = repositorioTiposAuto;
            this.repositorioMarcas = repositorioMarcas;
        }

        public IEnumerable<MarcaDto> ObtenerMarcas()
        {
            var marcas = repositorioMarcas.ObtenerMarcas();
            return Mapper.Map<IEnumerable<Marca>, IEnumerable<MarcaDto>>(marcas);
        }

        public IEnumerable<ProductoMinDto> ObtenerProductos(int? codigoRubro, string codigoMarca, int? codigoTipoAuto)
        {
            var productos = repositorioProductos.ObtenerProductosBase(codigoRubro, codigoMarca, codigoTipoAuto);
            return Mapper.Map<IEnumerable<ProductoBase>, IEnumerable<ProductoMinDto>>(productos);
        }

        public IEnumerable<RubroMinDto> ObtenerRubros()
        {
            var rubros = repositorioRubros.ObtenerRubrosBase();
            return Mapper.Map<IEnumerable<RubroBase>, IEnumerable<RubroMinDto>>(rubros);
        }

        public IEnumerable<TipoAutoMinDto> ObtenerTiposAuto()
        {
            var tiposAuto = repositorioTiposAuto.ObtenerTiposAuto();
            return Mapper.Map<IEnumerable<TipoAuto>, IEnumerable<TipoAutoMinDto>>(tiposAuto);
        }
    }
}