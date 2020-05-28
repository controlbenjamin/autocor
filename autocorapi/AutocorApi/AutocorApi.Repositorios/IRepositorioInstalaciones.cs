using AutocorApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioInstalaciones
    {

        IEnumerable<Instalacion> ObtenerInstalacionesPaginada(int pagina, int tamañoPagina);

        int CantidadProductos(string criterioBusqueda, DateTime? fechaInicialdeBusqueda);

        DateTime Insertar(Instalacion instalacion);

        void ActualizarEstado(string idInstalacion, bool estado);

        int ObtenerCantidadInstalacionesUsuario(int codigoCliente);

        IEnumerable<Instalacion> Buscar(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda);

        Instalacion Buscar(string instalacion);
    }
}
