using System;

namespace AutocorApi.Entidades
{
    public class PrecioProducto
    {
        public string CodigoPieza { get; set; }

        public decimal Precio { get; set; }
        public string OfertaTipo { get; set; }
        public decimal? OfertaImporte { get; set; }
        public DateTime? OfertaValidez { get; set; }
        public string OfertaSigno { get; set; }

        public decimal PrecioVigente => TieneOferta ? OfertaImporte.Value : Precio;

        public bool TieneOferta
        {
            get
            {
                if (OfertaValidez.HasValue && OfertaImporte.HasValue)
                    return this.OfertaValidez.Value > DateTime.Now && this.OfertaImporte.Value != 0.0m;

                return false;
            }
        }
    }
}