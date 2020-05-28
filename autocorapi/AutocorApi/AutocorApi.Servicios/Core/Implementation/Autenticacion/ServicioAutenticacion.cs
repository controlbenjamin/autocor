using System;
using AutocorApi.Entidades;
using AutocorApi.Entidades.Api;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation.Autenticacion
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        private const string CatalogoUsuario = "CatalogoUser";
        private const string CatalogoPassword = "CatalogoAutocor#Wachin123";

        private IRepositorioIniciosSesion repositorioIniciosSesion;
        private IRepositorioUsuarios repositorioUsuarios;
        private IRepositorioClientesAPI repositorioClientesAPI;
        private IRepositorioUsuariosWeb repositorioUsuariosWeb;

        public ServicioAutenticacion(
            IRepositorioIniciosSesion repoInicioSesion,
            IRepositorioUsuarios repositorioUsuarios,
            IRepositorioClientesAPI repositorioClientesAPI,
            IRepositorioUsuariosWeb repositorioUsuariosWeb)
        {
            this.repositorioIniciosSesion = repoInicioSesion;
            this.repositorioUsuarios = repositorioUsuarios;
            this.repositorioClientesAPI = repositorioClientesAPI;
            this.repositorioUsuariosWeb = repositorioUsuariosWeb;
        }

        public ClienteAPIDto ObtenerClienteAPI(Guid id)
        {
            var cliente = repositorioClientesAPI.ObtenerPorId(id);
            return Mapper.Map<ClienteAPI, ClienteAPIDto>(cliente);
        }

        public void RegistrarInicioSesion(InicioSesionDto inicioSesion)
        {
            InicioSesion nuevoInicioSesion = Mapper.Map<InicioSesionDto, InicioSesion>(inicioSesion);
            nuevoInicioSesion.Fecha = DateTime.Now;
            repositorioIniciosSesion.Insertar(nuevoInicioSesion);
        }

        public UsuarioDto ValidarUsuario(string nombreUsuario, string password)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                return null;

            var usuario = repositorioUsuarios.Buscar(nombreUsuario, password);
            var res = Mapper.Map<Usuario, UsuarioDto>(usuario);

            return res;
        }

        public UsuarioDto ValidarUsuarioWeb(string nombreUsuario, string password)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                return null;

            UsuarioDto res = null;

            if (nombreUsuario.Contains("@"))
            {
                //esta intentando logearse con un email
                res = AutenticarConMail(nombreUsuario, password);
            }
            else
            {
                //esta intentando entrar con nombre de usuario y cuit
                res = AutenticarConCuil(nombreUsuario, password);
            }

            return res;
        }

        private UsuarioDto AutenticarConCuil(string nombreUsuario, string password)
        {
            UsuarioDto res = null;
            if (password.Length < 11)
            {
                return null;
            }

            if (!password.Contains("-"))
            {
                password = string.Format("{0}-{1}-{2}", password.Substring(0, 2), password.Substring(2, 8), password.Substring(10, password.Length - 10));
            }

            var usuario = repositorioUsuarios.Buscar(nombreUsuario, password);
            res = Mapper.Map<Usuario, UsuarioDto>(usuario);
            if (res != null)
            {
                res.EstadoWeb = Dto.EstadoWeb.USUARIO_SIN_USUARIO_WEB;
                var usuarioWeb = repositorioUsuariosWeb.Buscar( res.CodigoCliente.ToString());
                if(usuarioWeb != null) res.EstadoWeb = Dto.EstadoWeb.USUARIO_WEB;
            }
            return res;
        }
        private UsuarioDto AutenticarConMail(string nombreUsuario, string password)
        {
            UsuarioDto res = null;
            var usuarioWeb = repositorioUsuariosWeb.AutenticarUsuario(nombreUsuario, password);
            if (usuarioWeb != null)
            {
                //el usuario existe
                var usuario = repositorioUsuarios.BuscarPorCodigoCliente(usuarioWeb.NroCliente);
                res = Mapper.Map<Usuario, UsuarioDto>(usuario);
                res.Estado = usuarioWeb.Estado;
            }
            return res;
        }


    }
}