using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Cache;

namespace AutocorApi.Repositorios.Dapper.Cache
{
    public class RepositorioMarcasCache : RepositorioMarcas, IRepositorioMarcas, IRepositorioCache
    {
        public RepositorioMarcasCache()
        {
        }

        public RepositorioMarcasCache(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public IMemoryCacher Cache { get; } = new MemoryCacher();

        public override Marca BuscarPorCodigo(string codigoMarca)
        {
            string cacheKey = "marca:" + codigoMarca;
            return Cache.GetOrAddAs(cacheKey, () => base.BuscarPorCodigo(codigoMarca), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<Marca> ObtenerMarcas()
        {
            string cacheKey = "marcas:todos";
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerMarcas(), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<Marca> ObtenerMarcasPorRubro(int codigoRubro)
        {
            string cacheKey = "marcas:rubro:" + codigoRubro;
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerMarcasPorRubro(codigoRubro), DateTimeOffset.UtcNow.AddMinutes(30));
        }
    }
}