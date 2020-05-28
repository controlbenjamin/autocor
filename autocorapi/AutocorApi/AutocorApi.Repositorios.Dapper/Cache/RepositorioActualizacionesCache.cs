using System;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Cache;

namespace AutocorApi.Repositorios.Dapper.Cache
{
    public class RepositorioActualizacionesCache : RepositorioActualizaciones, IRepositorioActualizaciones, IRepositorioCache
    {
        public RepositorioActualizacionesCache() : base()
        {
        }

        public RepositorioActualizacionesCache(IDbTransaction transaction) : base(transaction)
        {
        }

        public IMemoryCacher Cache { get; } = new MemoryCacher();

        public override Actualizacion ObtenerUltimaActualizacion()
        {
            string cacheKey = "actualizacion:ultima";
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerUltimaActualizacion(), DateTimeOffset.UtcNow.AddHours(12));
        }

        public override void Insertar(Actualizacion actualizacion)
        {
            Cache.Delete("actualizacion:ultima");
            base.Insertar(actualizacion);
        }
    }
}