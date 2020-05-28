using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;
using AutocorApi.Entidades;
using AutocorApi.Servicios.Dto.Utils;
using AutocorApi.Repositorios.Base;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioInstalaciones : IServicioInstalaciones
    {
        private readonly IRepositorioInstalaciones _repositorioInstalaciones;
        private readonly IRepositorioActivaciones _repositorioActivaciones;
        private IUoW uow;

        public ServicioInstalaciones(IRepositorioInstalaciones repositorioInstalaciones, IRepositorioActivaciones repositorioActivaciones, IUoW uow)
        {
            _repositorioInstalaciones = repositorioInstalaciones;
            _repositorioActivaciones = repositorioActivaciones;
            this.uow = uow;
        }

        public bool ActualizarEstado(string instalacion, bool estado)
        {
            bool res = true;
            var instalacionActualizada = _repositorioInstalaciones.Buscar(instalacion);
            var tieneActivacion = _repositorioActivaciones.BuscarActivacion(instalacionActualizada.NroCliente);
            try
            {
                uow.RepositorioInstalaciones.ActualizarEstado(instalacion, estado);
                if(tieneActivacion == null)uow.RepositorioActivaciones.Insertar(instalacionActualizada.NroCliente);
                uow.Commit();
            }
            catch(Exception e)
            {
                res = false;
            }
            return res;
        }

        public PagedResultDto<InstalacionDto> ObtenerInstalaciones(int pagina, int tamañoPagina)
        {
            return null;
            //turn Mapper.Map<IEnumerable<Instalacion>, IEnumerable<InstalacionDto>>(_repositorioInstalaciones.ObtenerInstalacionesPaginada(pagina, tamañoPagina));
        }

        public PagedResultDto<InstalacionDto> BuscarInstalaciones(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda)
        {
            var productos = Enumerable.Empty<InstalacionDto>();
            var cantidad = _repositorioInstalaciones.CantidadProductos(criterioBusqueda, fechaInicialdeBusqueda);
            if(cantidad != 0) productos = Mapper.Map<IEnumerable<Instalacion>, IEnumerable<InstalacionDto>>(_repositorioInstalaciones.Buscar(pagina, tamañoPagina, criterioBusqueda, fechaInicialdeBusqueda));
            return new PagedResultDto<InstalacionDto>(productos, pagina, tamañoPagina, cantidad);
        }

        public InstalacionDto NuevaInstalacion(int codigoCliente, string email, string nombrePC)
        {
            var cantidadInstalaciones = _repositorioInstalaciones.ObtenerCantidadInstalacionesUsuario(codigoCliente);
            string codigoInstalacion = codigoCliente + "-" + cantidadInstalaciones;
            Instalacion instalacion = new Instalacion()
            {
                IdInstalacion = codigoInstalacion,
                NroCliente = codigoCliente,
                Email = email,
                NombrePC = nombrePC
            };
            var fechaCarga = _repositorioInstalaciones.Insertar(instalacion);
            return new InstalacionDto()
            {
                IdInstalacion = codigoInstalacion,
                NroCliente = codigoCliente,
                Email = email,
                NombrePC = nombrePC,
                FechaHora = fechaCarga,
                Estado = null
            };
        }
    }
}
