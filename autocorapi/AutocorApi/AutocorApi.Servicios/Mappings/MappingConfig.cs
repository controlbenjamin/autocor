using AutocorApi.Entidades;
using AutocorApi.Entidades.Api;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Carrito;
using AutocorApi.Servicios.Dto.Clientes;
using AutocorApi.Servicios.Dto.Pedidos;
using AutocorApi.Servicios.Dto.Productos;
using AutoMapper;
using Ninject;

namespace AutocorApi.Servicios.Mappings
{
    /// <summary>
    /// Configuración de los mapeos de Entidad a DTO (Data Transfer Object) o viceversa.
    /// </summary>
    public class MappingConfig
    {
        public static void Initialize(IKernel kernel)
        {
            Mapper.Initialize(config =>
            {
                // Entidad -> DTO

                config.CreateMap<Producto, ProductoDto>();
                config.CreateMap<Producto, ProductoMinDto>();
                config.CreateMap<Producto, ProductoCarritoDto>();
                config.CreateMap<Producto, ProductoBaseDto>();
                config.CreateMap<Producto, StockDto>()
                    .ForMember(dst => dst.StockReal, opt => opt.MapFrom(src => src.StockReal));

                config.CreateMap<ProductoBase, ProductoMinDto>();

                config.CreateMap<Marca, MarcaDto>();
                config.CreateMap<TipoAuto, TipoAutoDto>();
                config.CreateMap<TipoAutoBase, TipoAutoMinDto>();
                config.CreateMap<TipoAuto, TipoAutoMinDto>()
                    .ForSourceMember(src => src.Marca, opt => opt.Ignore());

                config.CreateMap<Rubro, RubroDto>();
                config.CreateMap<RubroBase, RubroMinDto>();

                config.CreateMap<Cliente, ClienteDto>();
                config.CreateMap<Cliente, ClienteMinDto>()
                    .ForSourceMember(src => src.Configuracion, opts => opts.Ignore());

                config.CreateMap<ClienteBase, ClienteMinDto>();
                config.CreateMap<ConfiguracionCliente, ConfiguracionClienteDto>();

                config.CreateMap<Usuario, UsuarioDto>();
                config.CreateMap<Descarga, DescargaDto>();
                config.CreateMap<EstadoPedido, EstadoPedidoDto>();
                config.CreateMap<ParametroProducto, ParametroProductoDto>();
                config.CreateMap<ItemCarrito, ItemCarritoDto>();

                config.CreateMap<Pedido, PedidoDto>();

                config.CreateMap<DetallePedido, DetallePedidoDto>()
                    .ForMember(dst => dst.Rubro, opt => opt.MapFrom(src => src.Producto.Rubro.Descripcion)) // TODO: crítico
                    .ForSourceMember(src => src.Producto, opt => opt.Ignore());

                config.CreateMap<InicioSesion, InicioSesionDto>();
                config.CreateMap<Actualizacion, ActualizacionDto>();
                config.CreateMap<RefreshToken, RefreshTokenDto>();

                // DTO -> entidad

                config.CreateMap<EditItemCarritoDto, ItemCarrito>()
                    .ForMember(dst => dst.Cliente, opt => opt.Ignore())
                    .ForMember(dst => dst.Producto, opt => opt.Ignore());

                config.CreateMap<ConfiguracionClienteDto, ConfiguracionCliente>();

                config.CreateMap<InicioSesionDto, InicioSesion>();

                config.CreateMap<NuevoDetallePedidoDto, DetallePedido>()
                    .ForMember(dst => dst.Producto, opt => opt.Ignore())
                    .ForMember(dst => dst.SubTotal, opt => opt.Ignore());

                config.CreateMap<NuevoPedidoDto, Pedido>()
                    .ForMember(dst => dst.Observaciones, opt => opt.NullSubstitute(string.Empty));

                config.CreateMap<ActualizacionDto, Actualizacion>();

                config.CreateMap<ClienteAPIDto, ClienteAPI>();
                config.CreateMap<RefreshToken, RefreshTokenDto>();

                // DTO -> DTO

                config.CreateMap<ProductoDto, ProductoBaseDto>();
                config.CreateMap<ProductoDto, ProductoMinDto>();
            });
        }
    }
}