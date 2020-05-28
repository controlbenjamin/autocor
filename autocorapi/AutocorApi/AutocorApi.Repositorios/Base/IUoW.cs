using System;

namespace AutocorApi.Repositorios.Base
{
    public interface IUoW : IDisposable
    {
        // Repositories interfaces here as properties...

        IRepositorioClientes RepositorioClientes { get; }
        IRepositorioMarcas RepositorioMarcas { get; }
        IRepositorioProductos RepositorioProductos { get; }
        IRepositorioRubros RepositorioRubros { get; }
        IRepositorioTiposAuto RepositorioTiposAuto { get; }
        IRepositorioDescargas RepositorioDescargas { get; }
        IRepositorioEstadosPedidos RepositorioEstadosPedidos {get;}
        IRepositorioPedidos RepositorioPedidos { get; }
        IRepositorioDetallesPedidos RepositorioDetallesPedidos { get; }
        IRepositorioCarritos RepositorioCarritos { get; }
        IRepositorioConfiguracionesClientes RepositorioConfiguracionesClientes { get; }
        IRepositorioActivaciones RepositorioActivaciones { get; }
        IRepositorioInstalaciones RepositorioInstalaciones { get; }
        IRepositorioUsuariosWeb RepositorioUsuariosWeb { get; }

        void Commit();
    }
}