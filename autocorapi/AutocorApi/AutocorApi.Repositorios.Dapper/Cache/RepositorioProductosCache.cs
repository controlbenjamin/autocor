using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Cache;

namespace AutocorApi.Repositorios.Dapper.Cache
{
    public class RepositorioProductosCache : RepositorioProductos, IRepositorioProductos, IRepositorioCache
    {
        public RepositorioProductosCache()
            : base()
        {
        }

        public RepositorioProductosCache(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public IMemoryCacher Cache { get; } = new MemoryCacher();

        public override int ObtenerStockActual(string codigoPieza)
        {
            string cacheKey = "producto:stock:" + codigoPieza;
            return (int)Cache.GetOrAdd(cacheKey, () => base.ObtenerStockActual(codigoPieza), DateTimeOffset.UtcNow.AddMinutes(2));
        }

        public override bool VerificarExistenciaDeProductosEquivalentes(string codigoPieza)
        {
            string cacheKey = "producto:tiene_equivalencia:" + codigoPieza;
            return (bool)Cache.GetOrAdd(cacheKey, () => base.VerificarExistenciaDeProductosEquivalentes(codigoPieza), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<ParametroProducto> ObtenerParametrosProducto(string codigoPieza)
        {
            string cacheKey = $"producto:parametro:" + codigoPieza;
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerParametrosProducto(codigoPieza), DateTimeOffset.UtcNow.AddMinutes(30));
        }

        public override IEnumerable<Producto> ObtenerProductosMasPedidos(DateTime desde, DateTime hasta, int cantidad = 10, int? zona = null)
        {
            string cacheKey = $"productos:mas_pedidos:desde({desde}):hasta({hasta}):cantidad({cantidad}):zona({zona})";
            return Cache.GetOrAddAs(cacheKey, () => base.ObtenerProductosMasPedidos(desde, hasta, cantidad: cantidad, zona: zona), DateTimeOffset.UtcNow.AddHours(10));
        }
    }
}