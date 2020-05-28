namespace AutocorApi.Servicios.Dto
{
    public class TipoAutoDto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoMarca { get; set; }
        public virtual MarcaDto Marca { get; set; }
    }
}