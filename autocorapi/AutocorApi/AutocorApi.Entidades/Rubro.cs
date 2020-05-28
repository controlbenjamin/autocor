using System;
using System.Collections.Generic;
using System.Configuration;

namespace AutocorApi.Entidades
{
    public class RubroBase
    {
        private static readonly string _baseUrlImagen;

        static RubroBase()
        {
            _baseUrlImagen = ConfigurationManager.AppSettings["url_base_imagen_rubros"];
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string UrlImagen => $"{_baseUrlImagen}/{Codigo:000}/{Codigo:000}.jpg";
    }

    public class Rubro : RubroBase
    {
        public DateTime? Fecha { get; set; }
        public decimal? Descuento { get; set; }
        public virtual IEnumerable<string> ListaParametros { get; set; }
    }
}