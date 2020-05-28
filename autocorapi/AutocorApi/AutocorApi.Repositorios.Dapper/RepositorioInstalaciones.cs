using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
        
    public class RepositorioInstalaciones : RepositorioBase, IRepositorioInstalaciones
    {
        #region querys

        private static readonly string _QueryVersion = @"SELECT
                             ins.ID_INSTALACION as IdInstalacion,
                             ins.FECHA as FechaHora,
                             ins.CODCLI as NroCliente, 
                             cli.NOMBRE as NombreCliente,
                             ins.EMAIL as Email,
                             ins.NOMBRE_PC as NombrePC,
                             ins.ESTADO as Estado
                             from instalaciones ins join clientes cli on ins.CODCLI = cli.CODCLI";

        private static readonly string _QueryCount = "select count(*) from instalaciones ins join clientes cli on ins.CODCLI = cli.CODCLI";


        #endregion querys

        //TODO: refactorizar un poco, mover los strings.

        public RepositorioInstalaciones() : base()
        {
        }

        public RepositorioInstalaciones(IDbTransaction transaction) : base(transaction)
        {
        }

        public void ActualizarEstado(string idInstalacion, bool estado)
        {
            string sql = "update instalaciones set estado = @estado where id_instalacion = @idInstalacion";

            Connection.ExecuteScalar<DateTime>(sql, new { idInstalacion, estado }, transaction: Transaction);
        }


        public int ObtenerCantidadInstalacionesUsuario(int codigoCliente)
        {
            string query = _QueryCount + "where ins.CODCLI = @codigoCliente";
            return Connection.Query<int>(query, new { codigoCliente}, transaction: Transaction).FirstOrDefault();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public DateTime Insertar(Instalacion instalacion)
        {
            string sql = @"insert into instalaciones(ID_INSTALACION,CODCLI,EMAIL,NOMBRE_PC) 
                            values(@IdInstalacion,@NroCliente,@Email,@NombrePC)
                            select FECHA from instalaciones where ID_INSTALACION = @IdInstalacion";
                   
            return Connection.ExecuteScalar<DateTime>(sql, instalacion , transaction: Transaction);
        }

        public IEnumerable<Instalacion> ObtenerInstalacionesPaginada(int pagina, int tamañoPagina)
        {
            string query = _QueryVersion + " ORDER BY(FECHA) desc OFFSET @n ROWS FETCH NEXT @m ROWS ONLY";
            return Connection.Query<Instalacion>(query, new { n = (pagina - 1)*tamañoPagina, m=tamañoPagina } , transaction: Transaction);
        }

        public int CantidadProductos(string criterioBusqueda, DateTime? fechaInicialdeBusqueda)
        {
            string query = _QueryCount + " where 1=1";
            if (criterioBusqueda != null && criterioBusqueda.Length > 0)
            {
                query = query + @" and ( ID_INSTALACION LIKE CONCAT('%',@criterioBusqueda,'%')
                                  or ins.CODCLI like CONCAT('%',@criterioBusqueda,'%')
                                  or cli.NOMBRE like CONCAT('%',@criterioBusqueda,'%')
                                  or ins.EMAIL like CONCAT('%',@criterioBusqueda,'%')
                                  or ins.NOMBRE_PC like CONCAT('%',@criterioBusqueda,'%'))";
            }
            if (fechaInicialdeBusqueda != null)
            {
                query = query + " and FECHA >= @fechaInicialdeBusqueda";
            }
            return Connection.Query<int>(query, new {criterioBusqueda, fechaInicialdeBusqueda }, transaction: Transaction).FirstOrDefault();
        }

        public IEnumerable<Instalacion> Buscar(int pagina, int tamañoPagina, string criterioBusqueda, DateTime? fechaInicialdeBusqueda)
        {
            string query = _QueryVersion + " where 1=1";
            if(criterioBusqueda != null && criterioBusqueda.Length > 0)
            {
                query = query + @" and ( ID_INSTALACION LIKE CONCAT('%',@criterioBusqueda,'%')
                                  or ins.CODCLI like CONCAT('%',@criterioBusqueda,'%')
                                  or cli.NOMBRE like CONCAT('%',@criterioBusqueda,'%')
                                  or ins.EMAIL like CONCAT('%',@criterioBusqueda,'%')
                                  or ins.NOMBRE_PC like CONCAT('%',@criterioBusqueda,'%'))";
            }
            if(fechaInicialdeBusqueda != null)
            {
                query = query + " and FECHA >= @fechaInicialdeBusqueda";
            }
            query = query + " ORDER BY(FECHA) desc OFFSET @n ROWS FETCH NEXT @m ROWS ONLY";
            return Connection.Query<Instalacion>(query, new { n = (pagina - 1) * tamañoPagina, m = tamañoPagina, criterioBusqueda, fechaInicialdeBusqueda } , transaction: Transaction);
        }


        public Instalacion Buscar(string instalacion)
        {
            string query = _QueryVersion + " where 1=1";
            query = query + @" and ID_INSTALACION = @instalacion";
            return Connection.QueryFirstOrDefault<Instalacion>(query, new { instalacion} , transaction: Transaction);
        }
    }
}
