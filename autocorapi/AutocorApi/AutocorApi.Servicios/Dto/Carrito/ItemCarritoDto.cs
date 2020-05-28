using System;
using AutocorApi.Servicios.Dto.Productos;

namespace AutocorApi.Servicios.Dto.Carrito
{
    public class ItemCarritoDto
    {
        public string CodigoPieza { get; set; }
        public int CodigoCliente { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalPrecioOriginal { get; set; }
        public DateTime Fecha { get; set; }

        public virtual ProductoCarritoDto Producto { get; set; }
    }
}