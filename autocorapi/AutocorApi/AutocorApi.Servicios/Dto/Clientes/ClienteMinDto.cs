namespace AutocorApi.Servicios.Dto.Clientes
{
    public class ClienteMinDto
    {
        public int Codigo { get; set; }
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
        public int? CodigoZona { get; set; }
    }
}