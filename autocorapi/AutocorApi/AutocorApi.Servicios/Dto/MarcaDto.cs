namespace AutocorApi.Servicios.Dto
{
    public class MarcaDto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Letra => string.IsNullOrEmpty(Descripcion) ? string.Empty : Descripcion[0].ToString();
    }
}