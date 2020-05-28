using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioTiposAuto : IRepositorio
    {
        TipoAuto BuscarPorCodigo(int codigoTipoAuto);

        IEnumerable<TipoAuto> ObtenerTiposAuto();

        IEnumerable<TipoAuto> ObtenerTiposAutoPorMarca(string codigoMarca);

        TipoAutoBase BuscarBasePorCodigo(int codigoTipoAuto);

        IEnumerable<TipoAutoBase> ObtenerTiposAutoBase(string codigoMarca);
    }
}