namespace AutocorApi.Repositorios.Cache
{
    public interface IRepositorioCache
    {
        IMemoryCacher Cache { get; }
    }
}