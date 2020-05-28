using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Base;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Utils;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioUsuariosWeb : IServicioUsuariosWeb
    {
        private readonly IRepositorioUsuariosWeb _repositorioUsuariosWeb;
        private IUoW uow;
        public ServicioUsuariosWeb(IRepositorioUsuariosWeb repositorioUsuariosWeb, IUoW uow)
        {
            _repositorioUsuariosWeb = repositorioUsuariosWeb;
            this.uow = uow;
        }
        public bool ActualizarEstado(string nroCliente, bool estado)
        {
            bool res = true;
            try
            {
                uow.RepositorioUsuariosWeb.ActualizarEstado(nroCliente, estado);
                uow.Commit();
            }
            catch (Exception e)
            {
                res = false;
            }
            return res;
        }

        public PagedResultDto<UsuarioWebDto> BuscarUsuariosWeb(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda)
        {
            var usuarios = Enumerable.Empty<UsuarioWebDto>();
            var cantidad = _repositorioUsuariosWeb.CantidadUsuarios(criterioBusqueda, fechaInicialdeBusqueda);
            if (cantidad != 0) usuarios = Mapper.Map<IEnumerable<UsuarioWeb>, IEnumerable<UsuarioWebDto>>(_repositorioUsuariosWeb.Buscar(pagina, tamañoPagina, criterioBusqueda, fechaInicialdeBusqueda));
            return new PagedResultDto<UsuarioWebDto>(usuarios, pagina, tamañoPagina, cantidad);
        }

        public bool NuevoUsuariosWeb(int codigoCliente, string email, string pass)
        {
            UsuarioWeb usuarioAgregar = new UsuarioWeb()
            {
                NroCliente = codigoCliente,
                Email = email,
                Password = pass

            };
            return _repositorioUsuariosWeb.AgregarUsuarioWeb(usuarioAgregar);

        }

        public PagedResultDto<UsuarioWebDto> ObtenerUsuariosWeb(int pagina, int tamañoPagina)
        {
            return BuscarUsuariosWeb(pagina,tamañoPagina,"",null);
        }

        public bool CheckEstado(string nroCliente)
        {
            var usuario = _repositorioUsuariosWeb.Buscar(nroCliente);
            return usuario.Estado?? true;
        }

        public bool BorrarUsuarioWeb(string nroCliente)
        {
            return  _repositorioUsuariosWeb.EliminarUsuarioWeb(nroCliente);
        }

        public bool CheckExisteMail(string mail)
        {
            return _repositorioUsuariosWeb.ExisteMail(mail);
        }
    }
}
