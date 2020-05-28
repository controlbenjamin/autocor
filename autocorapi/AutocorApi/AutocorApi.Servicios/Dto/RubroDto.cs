using System;
using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto
{
    public class RubroDto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Descuento { get; set; }
        public virtual IEnumerable<string> ListaParametros { get; set; }
        public string UrlImagen { get; set; }
    }
}