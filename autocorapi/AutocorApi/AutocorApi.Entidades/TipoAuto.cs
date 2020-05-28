namespace AutocorApi.Entidades
{
    public class TipoAutoBase
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoMarca { get; set; }
    }

    public class TipoAuto : TipoAutoBase
    {
        // referencias
        public virtual Marca Marca { get; set; }
    }
}