using System;

namespace AutocorApi.Entidades
{
    public class ItemCarrito
    {
        public string CodigoPieza { get; set; }
        public int CodigoCliente { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Producto Producto { get; set; }

        public decimal SubTotal => Cantidad * Producto.PrecioVigente;
        public decimal SubTotalPrecioOriginal => Cantidad * Producto.Precio;
    }
}