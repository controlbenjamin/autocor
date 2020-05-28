using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioRubros : IServicioRubros
    {
        private IRepositorioRubros _repoRubros;

        public ServicioRubros(IRepositorioRubros repoRubros)
        {
            this._repoRubros = repoRubros;
        }

        public RubroMinDto BuscarMinPorCodigo(int codigoRubro)
        {
            var rubro = _repoRubros.ObtenerRubroBasePorCodigo(codigoRubro);
            return Mapper.Map<RubroBase, RubroMinDto>(rubro);
        }

        public RubroDto BuscarPorCodigo(int codigoRubro)
        {
            var rubro = _repoRubros.BuscarPorCodigo(codigoRubro);
            return Mapper.Map<Rubro, RubroDto>(rubro);
        }

        public IEnumerable<RubroDto> ObtenerRubros()
        {
            var rubros = _repoRubros.ObtenerRubros();
            return Mapper.Map<IEnumerable<Rubro>, IEnumerable<RubroDto>>(rubros);
        }

        public IEnumerable<RubroDto> ObtenerRubrosConIncorporaciones()
        {
            var rubros = _repoRubros.ObtenerRubrosIncorporaciones();
            return Mapper.Map<IEnumerable<Rubro>, IEnumerable<RubroDto>>(rubros);
        }

        public IEnumerable<RubroDto> ObtenerRubrosConOfertas()
        {
            var rubros = _repoRubros.ObtenerRubrosOferta();
            return Mapper.Map<IEnumerable<Rubro>, IEnumerable<RubroDto>>(rubros);
        }

        public IEnumerable<RubroMinDto> ObtenerRubrosMin()
        {
            var rubros = _repoRubros.ObtenerRubrosBase();
            return Mapper.Map<IEnumerable<RubroBase>, IEnumerable<RubroMinDto>>(rubros);
        }
    }
}