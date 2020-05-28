using AutocorApi.Servicios.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class ProductoCatalogoModel
    {
        public string CodigoPieza { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        //public decimal? OfertaImporte { get; set; }
        //public DateTime? OfertaValidez { get; set; }
        public string CodigoMarca { get; set; }
        public int CodigoTipoAuto { get; set; }
        public int CodigoRubro { get; set; }
        public int? Stock { get; set; }
        public MarcaDto Marca { get; set; }
        public RubroDto Rubro { get; set; }
        public TipoAutoDto TipoAuto { get; set; }
        public bool TieneOferta { get; set; }
        public decimal PrecioVigente { get; set; }
        public string UrlImagen { get; set; }
        public IEnumerable<ParametroProductoDto> Parametros { get; set; }
    }
}