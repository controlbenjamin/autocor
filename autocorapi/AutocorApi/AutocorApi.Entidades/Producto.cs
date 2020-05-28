using System;
using System.Collections.Generic;
using System.Configuration;

namespace AutocorApi.Entidades
{
    public class ProductoBase
    {
        public virtual string CodigoPieza { get; set; }
        public string NumeroOriginal { get; set; }
        public string Descripcion { get; set; }
        public string Articulo { get; set; }
    }

    public class Producto : ProductoBase
    {
        private static readonly int _minimoStock;
        private static readonly string _baseUrlImagen;
        private static readonly string _baseUrlImagenRubro;

        private PrecioProducto precio;

        static Producto()
        {
            _baseUrlImagen = ConfigurationManager.AppSettings["url_base_imagen_productos"];

            string stock = ConfigurationManager.AppSettings["stock_minimo"];
            int.TryParse(stock, out _minimoStock);
        }

        public Producto()
        {
            this.precio = new PrecioProducto();
        }

        public override string CodigoPieza
        {
            get { return base.CodigoPieza; }
            set { base.CodigoPieza = value; precio.CodigoPieza = value; }
        }

        public decimal Precio
        {
            get { return precio.Precio; }
            set { precio.Precio = value; }
        }

        public string OfertaTipo
        {
            get { return precio.OfertaTipo; }
            set { precio.OfertaTipo = value; }
        }

        public decimal? OfertaImporte
        {
            get { return precio.OfertaImporte; }
            set { precio.OfertaImporte = value; }
        }

        public DateTime? OfertaValidez
        {
            get { return precio.OfertaValidez; }
            set { precio.OfertaValidez = value; }
        }

        public string OfertaSigno
        {
            get { return precio.OfertaSigno; }
            set { precio.OfertaSigno = value; }
        }

        public string CodigoProductoMadre { get; set; }

        public string CodigoMarca { get; set; }
        public int CodigoTipoAuto { get; set; }
        public int CodigoRubro { get; set; }
        public int? Stock { get; set; }
        public int? StockMadre { get; set; }

        public virtual bool TieneEquivalencias { get; set; }

        public int UnidadVenta { get; set; }

        // referencias
        public virtual Marca Marca { get; set; }

        public virtual TipoAuto TipoAuto { get; set; }
        public virtual Rubro Rubro { get; set; }

        public virtual IEnumerable<ParametroProducto> Parametros { get; set; }

        // dinámicas
        public bool TieneOferta => precio.TieneOferta;
        public decimal PrecioVigente => precio.PrecioVigente;

        public string UrlImagen => $"{_baseUrlImagen}/{CodigoRubro:000}/{CodigoPieza}.jpg";

        public int StockReal
        {
            get 
            {
                if (!string.IsNullOrEmpty(CodigoProductoMadre))
                    return StockMadre ?? -1;

                return Stock ?? -1;
            }
        }

        public EstadoStock StockEstado => GetEstadoStock(StockReal);

        // métodos estáticos
        public static EstadoStock GetEstadoStock(int stock)
        {
            if (stock > 0)
            {
                if (stock > _minimoStock)
                    return EstadoStock.EN_STOCK;

                return EstadoStock.STOCK_MINIMO;
            }

            return EstadoStock.NO_DISPONIBLE;
        }
    }
}