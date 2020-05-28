namespace AutocorApi.Servicios.Dto.Productos
{
    public class ProductoCarritoDto
    {
        public string CodigoPieza { get; set; }
        public string NumeroOriginal { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public RubroDto Rubro { get; set; }
        public bool TieneOferta { get; set; }
        public decimal PrecioVigente { get; set; }
        public string UrlImagen { get; set; }
        public int UnidadVenta { get; set; }
    }
}