namespace AutocorApi.Models.Filtros
{
    public class FiltroPaginacion
    {
        public FiltroPaginacion()
        {
            Page = 1;
            Limit = 50;
        }

        public int Page { get; set; }
        public int Limit { get; set; }
    }
}