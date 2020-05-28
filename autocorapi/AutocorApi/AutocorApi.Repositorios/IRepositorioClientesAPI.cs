using System;
using System.Collections.Generic;
using AutocorApi.Entidades.Api;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioClientesAPI : IRepositorio
    {
        IEnumerable<ClienteAPI> ObtenerClientesAPI();

        ClienteAPI ObtenerPorId(Guid id);
    }
}