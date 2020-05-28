using System;
using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto.Productos
{
    public class ProductoBaseDto
    {
        public string CodigoPieza { get; set; }
        public string NumeroOriginal { get; set; }
        public string Articulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string CodigoMarca { get; set; }
        public int CodigoTipoAuto { get; set; }
        public int CodigoRubro { get; set; }
        public int? Stock { get; set; }
        public string StockEstado { get; set; }
        public MarcaDto Marca { get; set; }
        public RubroDto Rubro { get; set; }
        public TipoAutoDto TipoAuto { get; set; }
        public bool TieneOferta { get; set; }
        public decimal PrecioVigente { get; set; }
        public string UrlImagen { get; set; }
        public IEnumerable<ParametroProductoDto> Parametros { get; set; }
        public bool TieneEquivalencias { get; set; }
        public int UnidadVenta { get; set; }
    }

    public class ProductoDto : ProductoBaseDto
    {
        public string OfertaTipo { get; set; }
        public decimal? OfertaImporte { get; set; }
        public DateTime? OfertaValidez { get; set; }
        public string OfertaSigno { get; set; }
        public string CodigoProductoMadre { get; set; }
    }
}