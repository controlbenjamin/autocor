namespace AutocorApi.Servicios.Dto.Pedidos
{
    public class NuevoDetallePedidoDto
    {
        public string CodigoPieza { get; set; }
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
        // public decimal SubTotal => Cantidad * PrecioUnitario;
    }
}