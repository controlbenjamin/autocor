using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioUsuariosWeb
    {
        IEnumerable<UsuarioWeb> ObtenerUsuariosWeb (int pagina, int tamañoPagina);
        void ActualizarEstado(string numeroUsuario, bool estado);
        IEnumerable<UsuarioWeb> Buscar(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda);
        UsuarioWeb Buscar(string codcli);
        int CantidadUsuarios(string criterioBusqueda, DateTime? fechaInicialdeBusqueda);
        UsuarioWeb AutenticarUsuario(string email, string pass);
        bool AgregarUsuarioWeb(UsuarioWeb usuario);
        bool EliminarUsuarioWeb(string nroCliente);
        bool ExisteMail(string email);
    }
}
