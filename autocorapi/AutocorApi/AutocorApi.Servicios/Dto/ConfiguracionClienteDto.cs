namespace AutocorApi.Servicios.Dto
{
    public class ConfiguracionClienteDto
    {
        public int CodigoCliente { get; set; }
        public decimal Beneficio { get; set; }
        public decimal Descuento { get; set; }
        public bool IVA { get; set; }
    }
}