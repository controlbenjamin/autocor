namespace AutocorApi.Entidades
{
    public class ClienteBase
    {
        public int Codigo { get; set; }
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
        public int? CodigoZona { get; set; }
        public int? CodigoGira { get; set; }
    }

    public class Cliente : ClienteBase
    {
        public virtual ConfiguracionCliente Configuracion { get; set; }
    }
}