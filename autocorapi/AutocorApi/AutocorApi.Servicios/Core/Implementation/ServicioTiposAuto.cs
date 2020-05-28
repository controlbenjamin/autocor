using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioTiposAuto : IServicioTiposAuto
    {
        private IRepositorioTiposAuto _repoTiposAuto;

        public ServicioTiposAuto(IRepositorioTiposAuto repoTiposAuto)
        {
            this._repoTiposAuto = repoTiposAuto;
        }

        public TipoAutoMinDto BuscarMinPorCodigo(int codigoTipoAuto)
        {
            var tipoAuto = _repoTiposAuto.BuscarBasePorCodigo(codigoTipoAuto);
            return Mapper.Map<TipoAutoBase, TipoAutoMinDto>(tipoAuto);
        }

        public TipoAutoDto BuscarPorCodigo(int codigoTipoAuto)
        {
            var tipoAuto = _repoTiposAuto.BuscarPorCodigo(codigoTipoAuto);
            return Mapper.Map<TipoAuto, TipoAutoDto>(tipoAuto);
        }

        public IEnumerable<TipoAutoDto> ObtenerTiposAuto()
        {
            var tiposAuto = _repoTiposAuto.ObtenerTiposAuto();
            return Mapper.Map<IEnumerable<TipoAuto>, IEnumerable<TipoAutoDto>>(tiposAuto);
        }

        public IEnumerable<TipoAutoMinDto> ObtenerTiposAutoMin(string codigoMarca = null)
        {
            var tiposAuto = _repoTiposAuto.ObtenerTiposAutoBase(codigoMarca);
            return Mapper.Map<IEnumerable<TipoAutoBase>, IEnumerable<TipoAutoMinDto>>(tiposAuto);
        }

        public IEnumerable<TipoAutoDto> ObtenerTiposAutoPorMarca(string codigoMarca)
        {
            var tiposAuto = _repoTiposAuto.ObtenerTiposAutoPorMarca(codigoMarca);
            return Mapper.Map<IEnumerable<TipoAuto>, IEnumerable<TipoAutoDto>>(tiposAuto);
        }
    }
}