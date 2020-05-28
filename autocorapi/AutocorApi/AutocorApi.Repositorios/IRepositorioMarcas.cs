using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioMarcas : IRepositorio
    {
        Marca BuscarPorCodigo(string codigoMarca);

        IEnumerable<Marca> ObtenerMarcas();

        IEnumerable<Marca> ObtenerMarcasPorRubro(int codigoRubro);
    }
}