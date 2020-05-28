using System;

namespace AutocorApi.Models.Filtros
{
    public class FiltroCambioUsuario : FiltroPaginacion
    {
        public int? Cliente { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }
}