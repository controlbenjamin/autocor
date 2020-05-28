using Autocor.Catalogo.Web.Models.Filtros;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Productos;
using System.Collections.Generic;

namespace Autocor.Catalogo.Web.Models
{
    public class ProductosViewModel
    {
        public FiltroProductoModel FiltroProductos { get; set; }
        public IEnumerable<ProductoBaseDto> Productos { get; set; }
        public bool UltimaPagina { get; set; }

        public ProductosViewModel()
        {
            UltimaPagina = true;
        }
    }
}