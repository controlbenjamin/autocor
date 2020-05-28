namespace AutocorApi.Servicios.Dto.Pedidos
{
    public class DetallePedidoDto
    {
        public int IdPedido { get; set; }
        public string CodigoPieza { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Descripcion { get; set; }
        public string Rubro { get; set; }
        public decimal SubTotal { get; set; }
    }
}