namespace AutocorApi.Servicios.Dto.Clientes
{
    public class ClienteDto
    {
        public int Codigo { get; set; }
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
        public int? CodigoZona { get; set; }

        public ConfiguracionClienteDto Configuracion { get; set; }

        public override string ToString()
        {
            return string.Format("({0}) - {1}", Codigo, RazonSocial);
        }
    }
}