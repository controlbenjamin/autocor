namespace AutocorApi.Entidades
{
    public class ParametroProducto
    {
        public ParametroProducto()
        {
        }

        public ParametroProducto(string codigoPieza, int indice, string parametro, string valor)
        {
            this.CodigoPieza = codigoPieza;
            this.Indice = indice;
            this.Parametro = parametro;
            this.Valor = valor;
        }

        public string CodigoPieza { get; set; }
        public int Indice { get; set; }
        public string Parametro { get; set; }
        public string Valor { get; set; }
    }
}