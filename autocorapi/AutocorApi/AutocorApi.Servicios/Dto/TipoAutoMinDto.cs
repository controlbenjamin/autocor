namespace AutocorApi.Servicios.Dto
{
    public class TipoAutoMinDto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoMarca { get; set; }
        public string Letra => string.IsNullOrEmpty(Descripcion) ? string.Empty : Descripcion[0].ToString();
    }
}