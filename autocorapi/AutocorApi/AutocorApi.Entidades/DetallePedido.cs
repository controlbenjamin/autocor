namespace AutocorApi.Entidades
{
    public class DetallePedido
    {
        public int IdPedido { get; set; }
        public string CodigoPieza { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Descripcion { get; set; }

        public virtual Producto Producto { get; set; }

        public decimal SubTotal => PrecioUnitario * Cantidad;
    }
}