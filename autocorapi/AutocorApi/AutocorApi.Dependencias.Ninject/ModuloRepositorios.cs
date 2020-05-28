using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Base;
using AutocorApi.Repositorios.Dapper;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Dapper.Cache;
using Ninject.Modules;
using Ninject.Web.Common;

namespace AutocorApi.Dependencias.Ninject
{
    public class ModuloRepositorios : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepositorioProductos>().To<RepositorioProductosCache>().InRequestScope();
            Bind<IRepositorioMarcas>().To<RepositorioMarcasCache>().InRequestScope();
            Bind<IRepositorioRubros>().To<RepositorioRubrosCache>().InRequestScope();
            Bind<IRepositorioTiposAuto>().To<RepositorioTiposAutoCache>().InRequestScope();
            Bind<IRepositorioClientes>().To<RepositorioClientesCache>().InRequestScope();
            Bind<IRepositorioDescargas>().To<RepositorioDescargas>().InRequestScope();
            Bind<IRepositorioEstadosPedidos>().To<RepositorioEstadosPedidos>().InRequestScope();
            Bind<IRepositorioPedidos>().To<RepositorioPedidos>().InRequestScope();
            Bind<IRepositorioCarritos>().To<RepositorioCarritos>().InRequestScope();
            Bind<IRepositorioConfiguracionesClientes>().To<RepositorioConfiguracionesClientes>().InRequestScope();
            Bind<IRepositorioIniciosSesion>().To<RepositorioIniciosSesion>().InRequestScope();
            Bind<IRepositorioUsuarios>().To<RepositorioUsuarios>().InTransientScope();
            Bind<IRepositorioActualizaciones>().To<RepositorioActualizacionesCache>().InRequestScope();
            Bind<IRepositorioClientesAPI>().To<RepositorioClientesAPI>().InTransientScope();
            Bind<IRepositorioRefreshTokens>().To<RepositorioRefreshTokens>().InTransientScope();
            Bind<IRepositorioInstalaciones>().To<RepositorioInstalaciones>().InTransientScope();
            Bind<IRepositorioUsuariosWeb>().To<RepositorioUsuariosWeb>().InTransientScope();
            Bind<IRepositorioActivaciones>().To<RepositorioActivaciones>().InTransientScope();
            Bind<IUoW>().To<UoW>().InRequestScope();
        }
    }
}