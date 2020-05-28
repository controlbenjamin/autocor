using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioConfiguracionesClientes : RepositorioBase, IRepositorioConfiguracionesClientes
    {
        private static readonly string _QueryConfiguracionCliente;

        static RepositorioConfiguracionesClientes()
        {
            _QueryConfiguracionCliente = @"SELECT cc.CODCLI as CodigoCliente
                                              ,cc.BENEFICIO as Beneficio
                                              ,cc.DESCUENTO as Descuento
                                              ,cc.IVA as IVA
                                          FROM CLIENTES_CONFIGURACIONES cc ";
        }

        public RepositorioConfiguracionesClientes() : base()
        {
        }

        public RepositorioConfiguracionesClientes(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Actualizar(ConfiguracionCliente configuracion)
        {
            string sql = @"UPDATE CLIENTES_CONFIGURACIONES
                           SET  BENEFICIO = @Beneficio
                               ,DESCUENTO = @Descuento
                               ,IVA = @IVA
                         WHERE CODCLI = @CodigoCliente ";

            Connection.Execute(sql, configuracion, transaction: Transaction);
        }

        public ConfiguracionCliente BuscarPorCliente(int codigoCliente)
        {
            string query = _QueryConfiguracionCliente;

            query += "WHERE cc.CODCLI = @CodigoCliente ";

            return Connection.QuerySingleOrDefault<ConfiguracionCliente>(query, new { CodigoCliente = codigoCliente }, transaction: Transaction);
        }

        public void Insertar(ConfiguracionCliente configuracion)
        {
            string sql = @"INSERT INTO CLIENTES_CONFIGURACIONES
                               (CODCLI
                               ,BENEFICIO
                               ,DESCUENTO
                               ,IVA)
                         VALUES
                               (@CodigoCliente
                               ,@Beneficio
                               ,@Descuento
                               ,@IVA)";

            Connection.Execute(sql, configuracion, transaction: Transaction);
        }
    }
}