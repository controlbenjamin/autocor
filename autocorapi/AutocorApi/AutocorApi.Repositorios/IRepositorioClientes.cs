using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Utils;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioClientes : IRepositorio
    {
        Cliente BuscarPorCodigo(int codigoCliente);

        ClienteBase BuscarBasePorCodigo(int codigoCliente);

        Cliente BuscarPorCuit(string cuit);

        Cliente Buscar(int numero, string cuit);

        IEnumerable<Cliente> BuscarPorRazonSocial(string razonSocial, int? zona = null, int? gira = null);

        IEnumerable<ClienteBase> Buscar(int? zona = null, int? gira = null, PageConfig config = null);

        int CountBuscar(int? zona = null);

        bool VerificarExistencia(int codigoCliente);
    }
}