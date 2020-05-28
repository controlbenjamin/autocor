using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioUsuariosWeb : RepositorioBase, IRepositorioUsuariosWeb
    {
        #region querys

        private static readonly string _QueryVersion = @"SELECT
                             uw.CODCLI as NroCliente,
                             uw.fecha as FechaHora,
                             cli.NOMBRE as NombreCliente,
                             uw.email as Email,
                             uw.ESTADO as Estado
                             from usuarios_web uw join clientes cli on uw.CODCLI = cli.CODCLI";

        private static readonly string _AllInfo = @"SELECT
                             uw.CODCLI as NroCliente,
                             uw.fecha as FechaHora,
                             cli.NOMBRE as NombreCliente,
                             uw.email as Email,
                             uw.ESTADO as Estado,
                             uw.password as Password
                             from usuarios_web uw join clientes cli on uw.CODCLI = cli.CODCLI";

        private static readonly string _QueryCount = "select count(*) from usuarios_web uw join clientes cli on uw.CODCLI = cli.CODCLI";


        #endregion querys
        public RepositorioUsuariosWeb() : base()
        {
        }

        public RepositorioUsuariosWeb(IDbTransaction transaction) : base(transaction)
        {
        }
        public IEnumerable<UsuarioWeb> ObtenerUsuariosWeb(int pagina, int tamañoPagina)
        {
            string query = _QueryVersion + " ORDER BY(FECHA) desc OFFSET @n ROWS FETCH NEXT @m ROWS ONLY";
            return Connection.Query<UsuarioWeb>(query, new { n = (pagina - 1) * tamañoPagina, m = tamañoPagina }, transaction: Transaction);
        }
        public void ActualizarEstado(string numeroUsuario, bool estado)
        {
            string sql = "update usuarios_web set estado = @estado where CODCLI = @numeroUsuario";
            Connection.ExecuteScalar<DateTime>(sql, new { numeroUsuario, estado }, transaction: Transaction);
        }

        public IEnumerable<UsuarioWeb> Buscar(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda)
        {
            string query = _QueryVersion + " where 1=1";
            if (criterioBusqueda != null && criterioBusqueda.Length > 0)
            {
                query = query + @" and (uw.CODCLI like CONCAT('%',@criterioBusqueda,'%')
                                  or cli.NOMBRE like CONCAT('%',@criterioBusqueda,'%')
                                  or uw.email like CONCAT('%',@criterioBusqueda,'%'))";
            }
            if (fechaInicialdeBusqueda != null)
            {
                query = query + " and FECHA >= @fechaInicialdeBusqueda";
            }
            query = query + " ORDER BY(FECHA) desc OFFSET @n ROWS FETCH NEXT @m ROWS ONLY";
            return Connection.Query<UsuarioWeb>(query, new { n = (pagina - 1) * tamañoPagina, m = tamañoPagina, criterioBusqueda, fechaInicialdeBusqueda }, transaction: Transaction);
        }

        public UsuarioWeb Buscar(string codcli)
        {
            string query = _QueryVersion + " where 1=1";
            query = query + @" and uw.codcli = @codcli";
            return Connection.QueryFirstOrDefault<UsuarioWeb>(query, new { codcli }, transaction: Transaction);
        }
        public int CantidadUsuarios(string criterioBusqueda, DateTime? fechaInicialdeBusqueda)
        {
            string query = _QueryCount + " where 1=1";
            if (criterioBusqueda != null && criterioBusqueda.Length > 0)
            {
                query = query + @" and (uw.CODCLI like CONCAT('%',@criterioBusqueda,'%')
                                  or cli.NOMBRE like CONCAT('%',@criterioBusqueda,'%')
                                  or uw.email like CONCAT('%',@criterioBusqueda,'%'))";
            }
            if (fechaInicialdeBusqueda != null)
            {
                query = query + " and FECHA >= @fechaInicialdeBusqueda";
            }
            return Connection.Query<int>(query, new { criterioBusqueda, fechaInicialdeBusqueda }, transaction: Transaction).FirstOrDefault();
        }

        public UsuarioWeb AutenticarUsuario(string email, string pass)
        {
            var usuario = ObtenerUsuarioPorMail(email);
            if (usuario == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(pass, usuario.Password)) return null;
            usuario.Password = null;
            return usuario;
        }

        private UsuarioWeb ObtenerUsuarioPorMail(string email)
        {
            string query = _AllInfo;
            query = query + @" where email = @email";
            return Connection.Query<UsuarioWeb>(query, new {email}, transaction: Transaction).FirstOrDefault();
        }
        private UsuarioWeb ObtenerUsuarioPorNumero(string nroCliente)
        {
            string query = _AllInfo;
            query = query + @" where cli.CODCLI = @nroCliente";
            return Connection.Query<UsuarioWeb>(query, new { nroCliente }, transaction: Transaction).FirstOrDefault();
        }


        //TODO: diferenciar error de BD y error de usuario ya existente
        public bool AgregarUsuarioWeb(UsuarioWeb usuario)
        {
            bool respuesta = true;
            if (ObtenerUsuarioPorMail(usuario.Email) != null) { return false; }
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            //benja -> codigo viejo
            //string sql = "insert into usuarios_web(CODCLI,email,Password) values (@NroCliente, @Email, @Password)";

            //benja --> se coloca por defecto estado True, para que no se pida autorizacion
            usuario.Estado = true;
            string sql = "insert into usuarios_web(CODCLI, email, Password, estado) values (@NroCliente, @Email, @Password, @Estado)";

            try
            {
                Connection.Execute(sql, usuario, transaction: Transaction);
            }catch(Exception e)
            {
                respuesta = false;
            } 
            return respuesta;
        }

        public bool EliminarUsuarioWeb(string nroCliente)
        {
            bool respuesta = true;
            if (ObtenerUsuarioPorNumero(nroCliente) == null) { return false; }
            string sql = "delete from usuarios_web where CODCLI = @nroCliente";

            try
            {
                Connection.Execute(sql,new { nroCliente }, transaction: Transaction);
            }
            catch (Exception e)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public bool ExisteMail(string mail)
        {
            string sql = "select email from usuarios_web where email = @mail";
            var encontrado = Connection.Query(sql, new { mail }, transaction: Transaction).FirstOrDefault();
            return encontrado != null;
        }

    }
}
