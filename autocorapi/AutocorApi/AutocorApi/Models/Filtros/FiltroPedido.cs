using System;

namespace AutocorApi.Models.Filtros
{
    public class FiltroPedido : FiltroPaginacion
    {
        public int? Cliente { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public int? Estado { get; set; }
        public int? ZonaCliente { get; set; }
    }
}