using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioActivaciones : RepositorioBase, IRepositorioActivaciones
    {

        public RepositorioActivaciones() : base()
        {
        }

        public RepositorioActivaciones(IDbTransaction transaction) : base(transaction)
        {
        }

            public Activacion BuscarActivacion(int idUsuario)
        {
            string sql = @"SELECT CODCLI, fecha
                            FROM ACTIVACIONES_CLIENTES
                            WHERE CODCLI = @idUsuario";
            return Connection.QuerySingleOrDefault<Activacion>(sql, new {idUsuario}, transaction: Transaction);

        }

        public void Insertar(int idUsuario)
        {
            string sql = @"insert into ACTIVACIONES_CLIENTES(CODCLI) values(@idUsuario)";
            Connection.Execute(sql, new {idUsuario}, transaction: Transaction);
        }

    }
}
