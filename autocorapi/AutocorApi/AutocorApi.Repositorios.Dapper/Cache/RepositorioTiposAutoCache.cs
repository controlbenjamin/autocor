using AutocorApi.Entidades;
using AutocorApi.Repositorios.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Repositorios.Dapper.Cache
{
    public class RepositorioTiposAutoCache : RepositorioTiposAuto, IRepositorioTiposAuto, IRepositorioCache
    {
        public RepositorioTiposAutoCache()
        {
        }

        public RepositorioTiposAutoCache(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public IMemoryCacher Cache { get; } = new MemoryCacher();

        public override TipoAutoBase BuscarBasePorCodigo(int codigoTipoAuto)
        {
            string cacheKey = "tipoAuto_base:" + codigoTipoAuto;
            return Cache.GetOrAddAs(cacheKey, () => base.BuscarBasePorCodigo(codigoTipoAuto), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override TipoAuto BuscarPorCodigo(int codigoTipoAuto)
        {
            string cacheKey = "tipoAuto:" + codigoTipoAuto;
            return Cache.GetOrAddAs(cacheKey, () => base.BuscarPorCodigo(codigoTipoAuto), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<TipoAutoBase> ObtenerTiposAutoBase(string codigoMarca)
        {
            string cacheKey = "tipoAuto_base:marca:" + codigoMarca;
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerTiposAutoBase(codigoMarca), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        protected override IEnumerable<TipoAuto> _ObtenerTiposAuto(int? codigoTipoAuto = null, string codigoMarca = null)
        {
            string cacheKey = "tipoAuto:" + codigoTipoAuto + "marca:" + codigoMarca;
            return Cache.GetOrAddAs(cacheKey, () => base._ObtenerTiposAuto(codigoTipoAuto: codigoTipoAuto, codigoMarca: codigoMarca), DateTimeOffset.UtcNow.AddMinutes(30));
        }
    }
}
