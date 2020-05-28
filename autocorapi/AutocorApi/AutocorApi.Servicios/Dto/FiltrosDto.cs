using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto
{
    public class FiltrosDto
    {
        public IEnumerable<RubroMinDto> Rubros { get; set; }
        public IEnumerable<MarcaDto> Marcas { get; set; }
        public IEnumerable<TipoAutoMinDto> TiposAuto { get; set; }
    }
}