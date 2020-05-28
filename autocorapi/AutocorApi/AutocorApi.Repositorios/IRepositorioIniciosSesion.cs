using System;
using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Utils;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioIniciosSesion : IRepositorio
    {
        void Insertar(InicioSesion inicioSesion);

        IEnumerable<InicioSesion> ObtenerIniciosSesion(int? codigoCliente, string tipoCatalogo, 
            DateTime? fechaDesde, DateTime? fechaHasta, int? codigoClienteAnterior, bool soloCambiosUsuario,
            PageConfig page = null);

        int CountObtenerIniciosSesion(int? codigoCliente, string tipoCatalogo,
           DateTime? fechaDesde, DateTime? fechaHasta, int? codigoClienteAnterior, bool soloCambiosUsuario);
    }
}