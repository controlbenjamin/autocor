using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Utils;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioClientes : RepositorioBase, IRepositorioClientes
    {
        #region Queries

        private const string _QueryCliente =
            @"SELECT c.CODCLI as Codigo,
                     c.NOMBRE as RazonSocial,
                     c.CUIT as CUIT,
                     c.ZON_CODIGO as CodigoZona,
                     c.GIR_CODIGO as CodigoGira,
                     
                     cc.CODCLI as CodigoCliente,
                     cc.BENEFICIO as Beneficio,
                     cc.DESCUENTO as Descuento,
                     cc.IVA as IVA

             FROM CLIENTES c
             LEFT JOIN CLIENTES_CONFIGURACIONES cc ON c.CODCLI = cc.CODCLI ";

        private const string _QueryClienteBase =
            @"SELECT c.CODCLI as Codigo,
                     c.NOMBRE as RazonSocial,
                     c.CUIT as CUIT,
                     c.ZON_CODIGO as CodigoZona,
                     c.GIR_CODIGO as CodigoGira
              FROM CLIENTES c ";

        private const string _QueryCountBuscar =
            "SELECT COUNT(*) FROM CLIENTES c ";

        #endregion

        public RepositorioClientes() : base()
        {
        }

        public RepositorioClientes(IDbTransaction transaction) : base(transaction)
        {
        }

        public Cliente Buscar(int numero, string cuit)
        {
            return this._Buscar(codigo: numero, cuit: cuit);
        }

        public Cliente BuscarPorCuit(string cuit)
        {
            return this._Buscar(cuit: cuit);
        }

        public virtual Cliente BuscarPorCodigo(int codigoCliente)
        {
            return this._Buscar(codigo: codigoCliente);
        }

        public IEnumerable<Cliente> BuscarPorRazonSocial(string razonSocial, int? zona = null, int? gira = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string query = _QueryCliente + " WHERE 1=1 AND c.NOMBRE LIKE @Nombre ";

            parameters.Add("Nombre", "%" + razonSocial + "%");

            if(zona.HasValue)
            {
                query += " AND c.ZON_CODIGO = @zona";
                parameters.Add("zona", zona.Value);
            }

            if(gira.HasValue)
            {
                query += " AND c.GIR_CODIGO = @gira";
                parameters.Add("gira", gira.Value);
            }

            query += " ORDER BY c.NOMBRE ";

            return Connection.Query<Cliente, ConfiguracionCliente, Cliente>(query, (cliente, configuracion) => MapearCliente(cliente, configuracion),
                param: parameters, transaction: Transaction, splitOn: "CodigoCliente");
        }

        public bool VerificarExistencia(int codigoCliente)
        {
            string query = "SELECT 1 FROM CLIENTES c WHERE c.CODCLI = @NumeroCliente";

            return Connection.Query(query, new { NumeroCliente = codigoCliente }, transaction: Transaction)
                .Count() > 0;
        }

        public IEnumerable<ClienteBase> Buscar(int? zona = null, int? gira = null, PageConfig config = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string query = _QueryClienteBase + " WHERE 1=1 ";

            if (zona.HasValue)
            {
                query += " AND c.ZON_CODIGO = @zona ";
                parameters.Add("zona", zona.Value);
            }

            if (gira.HasValue)
            {
                query += " AND c.GIR_CODIGO = @gira ";
                parameters.Add("gira", gira.Value);
            }

            query += "ORDER BY c.NOMBRE ASC ";

            // agregar paginación
            if (config != null)
            {
                query += "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ";
                parameters.AddDynamicParams(new { config.Offset, config.PageSize });
            }

            return Connection.Query<ClienteBase>(query, param: parameters, transaction: Transaction);
        }

        public int CountBuscar(int? zona = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string query = _QueryCountBuscar + " WHERE 1=1 ";

            if (zona.HasValue)
            {
                parameters.Add("zona", zona.Value);
                query += " AND c.ZON_CODIGO = @zona ";
            }

            return Connection.ExecuteScalar<int>(query, param: parameters, transaction: Transaction);
        }

        public ClienteBase BuscarBasePorCodigo(int codigoCliente)
        {
            string query = _QueryClienteBase + " WHERE c.CODCLI = @codigoCliente";
            return Connection.QueryFirstOrDefault<ClienteBase>(query, param: new { codigoCliente }, transaction: Transaction);
        }

        // private methods
        private Cliente _Buscar(int? codigo = null, string cuit = null)
        {
            var parametros = new DynamicParameters();

            string query = _QueryCliente + " WHERE 1=1 ";

            if (codigo.HasValue)
            {
                query += " AND c.CODCLI = @NumeroCliente ";
                parametros.Add("NumeroCliente", codigo.Value);
            }

            if (!string.IsNullOrEmpty(cuit))
            {
                query += " AND c.CUIT = @Cuit ";
                parametros.Add("Cuit", cuit);
            }

            return Connection.Query<Cliente, ConfiguracionCliente, Cliente>(query, (cliente, configuracion) => MapearCliente(cliente, configuracion),
                parametros, transaction: Transaction, splitOn: "CodigoCliente").FirstOrDefault();
        }

        private Cliente MapearCliente(Cliente cliente, ConfiguracionCliente configuracion)
        {
            configuracion = configuracion ?? new ConfiguracionCliente();

            configuracion.CodigoCliente = cliente.Codigo;
            cliente.Configuracion = configuracion;
            return cliente;
        }
    }
}