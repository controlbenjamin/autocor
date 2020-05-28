using AutocorApi.Repositorios.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios.Dapper.Cache
{
    public class RepositorioClientesCache : RepositorioClientes, IRepositorioClientes, IRepositorioCache
    {
        public RepositorioClientesCache() : base()
        {
        }

        public RepositorioClientesCache(IDbTransaction transaction) : base(transaction)
        {
        }

        public IMemoryCacher Cache { get; } = new MemoryCacher();

        public override Cliente BuscarPorCodigo(int codigoCliente)
        {
            string cacheKey = "clientes:" + codigoCliente;
            return Cache.GetOrAddAs(cacheKey, () => base.BuscarPorCodigo(codigoCliente), DateTimeOffset.UtcNow.AddMinutes(2));
        }
    }
}
