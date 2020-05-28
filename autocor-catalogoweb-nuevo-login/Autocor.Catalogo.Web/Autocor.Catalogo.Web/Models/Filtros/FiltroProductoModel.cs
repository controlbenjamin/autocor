using AutocorApi.Servicios.Dto;
using System.Collections.Generic;

namespace Autocor.Catalogo.Web.Models.Filtros
{
    public class FiltroProductoModel
    {
        public string CriterioBusqueda { get; set; }
        public int? CodigoRubro { get; set; }
        public string CodigoMarca { get; set; }
        public int? CodigoTipoAuto { get; set; }

        public int Pagina { get; set; }

        public IEnumerable<MarcaDto> Marcas { get; set; }
        public IEnumerable<RubroDto> Rubros { get; set; }
        public IEnumerable<TipoAutoDto> Tipos { get; set; }


        public FiltroProductoModel()
        {
            this.Pagina = 1;
        }

        public bool BuscaProducto()
        {
            if (!string.IsNullOrEmpty(CriterioBusqueda)) return true;

            if (CodigoRubro.HasValue) return true;

            if (!string.IsNullOrEmpty(CodigoMarca)) return true;

            if (CodigoTipoAuto.HasValue) return true;

            return false;
        }


    }
}