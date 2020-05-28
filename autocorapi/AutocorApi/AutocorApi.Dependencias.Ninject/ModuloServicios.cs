using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Core.Implementation;
using AutocorApi.Servicios.Core.Implementation.Autenticacion;
using AutocorApi.Servicios.Core.Implementation.Pedidos;
using AutocorApi.Servicios.Core.Pedidos;
using AutocorApi.Servicios.Email;
using AutocorApi.Servicios.Email.Implementation;
using Ninject.Modules;
using Ninject.Web.Common;

namespace AutocorApi.Dependencias.Ninject
{
    public class ModuloServicios : NinjectModule
    {
        public override void Load()
        {
            Bind<IServicioAutenticacion>().To<ServicioAutenticacion>();
            Bind<IServicioProductos>().To<ServicioProductos>();
            Bind<IServicioClientes>().To<ServicioClientes>();
            Bind<IServicioDescargas>().To<ServicioDescargas>();
            Bind<IServicioConsultaPedidos>().To<ServicioConsultaPedidos>();
            Bind<IServicioCarrito>().To<ServicioCarrito>();
            Bind<IServicioTiposAuto>().To<ServicioTiposAuto>();
            Bind<IServicioRubros>().To<ServicioRubros>();
            Bind<IServicioMarcas>().To<ServicioMarcas>();
            Bind<IServicioPedidos>().To<ServicioPedidos>();
            Bind<IServicioIniciosSesion>().To<ServicioIniciosSesion>();
            Bind<IServicioActualizaciones>().To<ServicioActualizaciones>();
            Bind<IServicioMiniCatalogo>().To<ServicioMiniCatalogo>();
            Bind<IServicioUsuarios>().To<ServicioUsuarios>();
            Bind<IServicioToken>().To<ServicioToken>();

            Bind<IServicioEmail>().To<ServicioEnvioEmail>();
            Bind<IConfiguracionEmail>().To<ConfiguracionEmailDefault>();
            Bind<IEnviadorEmail>().To<EnviadorEmail>();

            Bind<IServicioInstalaciones>().To<ServicioInstalaciones>();
            Bind<IServicioUsuariosWeb>().To<ServicioUsuariosWeb>();

        }
    }
}