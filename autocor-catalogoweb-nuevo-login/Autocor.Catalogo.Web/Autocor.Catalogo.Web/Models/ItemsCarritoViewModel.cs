using AutocorApi.Servicios.Dto.Carrito;
using AutocorApi.Servicios.Dto.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class ItemsCarritoViewModel
    {
        public IEnumerable<ItemCarritoDto> Items { get; set; }
        public decimal Total
        {
            get
            {
                if (Items == null)
                    return 0m;

                decimal total = 0m;

                foreach (var item in Items)
                {
                    total += item.SubTotal;
                }

                return total;
            }
        }

        public decimal TotalPrecioOriginal
        {
            get
            {
                if (Items == null)
                    return 0m;

                decimal total = 0m;

                foreach (var item in Items)
                {
                    total += item.SubTotalPrecioOriginal;
                }

                return total;
            }
        }
    }
}