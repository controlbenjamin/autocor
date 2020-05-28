using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades.Api;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioClientesAPI : RepositorioBase, IRepositorioClientesAPI
    {
        private static readonly string _QueryVersion;

        static RepositorioClientesAPI()
        {
            _QueryVersion = @"SELECT
                                 IdCliente as Id,
                                 Nombre,
                                 Activo,
                                 Roles as DescripcionRoles
                             FROM ApiClients ";
        }

        public RepositorioClientesAPI() : base()
        {
        }

        public RepositorioClientesAPI(IDbTransaction transaction) : base(transaction)
        {
        }

        public IEnumerable<ClienteAPI> ObtenerClientesAPI()
        {
            return Connection.Query<ClienteAPI>(_QueryVersion, transaction: Transaction);
        }

        public ClienteAPI ObtenerPorId(Guid id)
        {
            string query = _QueryVersion + " WHERE IdCliente = @id";
            return Connection.QuerySingleOrDefault<ClienteAPI>(query, new { id }, transaction: Transaction);
        }
    }
}