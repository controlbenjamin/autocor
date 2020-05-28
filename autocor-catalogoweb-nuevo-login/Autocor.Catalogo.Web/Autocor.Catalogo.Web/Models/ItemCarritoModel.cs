using AutocorApi.Servicios.Dto.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class ItemCarritoModel
    {
        public string CodigoPieza { get; set; }
        public int CodigoCliente { get; set; }
        public int Cantidad { get; set; }

        public virtual ProductoDto Producto { get; set; }
    }
}