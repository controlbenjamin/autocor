using System.Collections.Generic;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioDescargas : IRepositorio
    {
        Descarga ObtenerPorId(int idDescarga);

        IEnumerable<Descarga> ObtenerDescargas();
    }
}