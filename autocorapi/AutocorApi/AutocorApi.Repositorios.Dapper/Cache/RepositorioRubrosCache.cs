using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Cache;

namespace AutocorApi.Repositorios.Dapper.Cache
{
    public class RepositorioRubrosCache : RepositorioRubros, IRepositorioRubros, IRepositorioCache
    {
        private IMemoryCacher _cache = new MemoryCacher();

        public RepositorioRubrosCache() : base()
        {
        }

        public RepositorioRubrosCache(IDbTransaction transaction) : base(transaction)
        {
        }

        public IMemoryCacher Cache => _cache;

        public override IEnumerable<string> ObtenerParametrosPorRubro(int codigoRubro)
        {
            string cacheKey = "rubro:parametros:" + codigoRubro;
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerParametrosPorRubro(codigoRubro), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override Rubro BuscarPorCodigo(int codigoRubro)
        {
            string cacheKey = "rubro:" + codigoRubro;
            return Cache.GetOrAddAs(cacheKey, () => base.BuscarPorCodigo(codigoRubro), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<Rubro> ObtenerRubros()
        {
            string cacheKey = "rubros:todos";
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerRubros(), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<RubroBase> ObtenerRubrosBase()
        {
            string cacheKey = "rubros_base";
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerRubrosBase(), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override RubroBase ObtenerRubroBasePorCodigo(int codigoRubro)
        {
            string cacheKey = "rubro_base:" + codigoRubro;
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerRubroBasePorCodigo(codigoRubro), DateTimeOffset.UtcNow.AddMinutes(30));
        }
    }
}