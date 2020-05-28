using AutocorApi.Extensions;

namespace AutocorApi.Models.Filtros
{
    public class FiltroProducto : FiltroPaginacion
    {
        public string Descripcion { get; set; }
        public int? Rubro { get; set; }
        public string Marca { get; set; }
        public int? TipoAuto { get; set; }
    }
}