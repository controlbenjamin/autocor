using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioMarcas : IServicioMarcas
    {
        private IRepositorioMarcas _repoMarcas;

        public ServicioMarcas(IRepositorioMarcas repoMarcas)
        {
            this._repoMarcas = repoMarcas;
        }

        public MarcaDto BuscarPorCodigo(string codigoMarca)
        {
            var marca = _repoMarcas.BuscarPorCodigo(codigoMarca);
            return Mapper.Map<Marca, MarcaDto>(marca);
        }

        public IEnumerable<MarcaDto> ObtenerMarcas()
        {
            var marcas = _repoMarcas.ObtenerMarcas();
            return Mapper.Map<IEnumerable<Marca>, IEnumerable<MarcaDto>>(marcas);
        }

        public IEnumerable<MarcaDto> ObtenerMarcas(int? codigoRubro)
        {
            if (codigoRubro.HasValue)
                return ObtenerMarcasPorRubro(codigoRubro.Value);

            return ObtenerMarcas();
        }

        public IEnumerable<MarcaDto> ObtenerMarcasPorRubro(int codigoRubro)
        {
            var marcas = _repoMarcas.ObtenerMarcasPorRubro(codigoRubro);

            return Mapper.Map<IEnumerable<Marca>, IEnumerable<MarcaDto>>(marcas);
        }
    }
}