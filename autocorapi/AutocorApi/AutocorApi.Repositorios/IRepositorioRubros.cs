using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioRubros : IRepositorio
    {
        Rubro BuscarPorCodigo(int codigoRubro);

        IEnumerable<string> ObtenerParametrosPorRubro(int codigoRubro);

        IEnumerable<Rubro> ObtenerRubros();

        IEnumerable<Rubro> ObtenerRubros(string codigoMarca, string codigoTipoAuto);

        IEnumerable<Rubro> ObtenerRubrosIncorporaciones();

        IEnumerable<Rubro> ObtenerRubrosOferta();

        IEnumerable<RubroBase> ObtenerRubrosBase();

        RubroBase ObtenerRubroBasePorCodigo(int codigoRubro);
    }
}