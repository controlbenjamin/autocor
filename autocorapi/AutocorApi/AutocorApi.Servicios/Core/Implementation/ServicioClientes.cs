using System;
using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Utils;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Clientes;
using AutocorApi.Servicios.Dto.Utils;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioClientes : IServicioClientes
    {
        private IRepositorioClientes repositorioClientes;
        private IRepositorioConfiguracionesClientes repositorioConfiguracion;

        public ServicioClientes(IRepositorioClientes repoClientes, 
            IRepositorioConfiguracionesClientes repoConfiguracion) 
        {
            this.repositorioClientes = repoClientes;
            this.repositorioConfiguracion = repoConfiguracion;
        }

        public void GuardarConfiguracion(ConfiguracionClienteDto configuracion)
        {
            var nuevaConfiguracion = Mapper.Map<ConfiguracionClienteDto, ConfiguracionCliente>(configuracion);

            // ver si existe
            var configActual = repositorioConfiguracion.BuscarPorCliente(configuracion.CodigoCliente);

            if (configActual == null)
            {
                // es una nueva
                repositorioConfiguracion.Insertar(nuevaConfiguracion);
            }

            this.repositorioConfiguracion.Actualizar(nuevaConfiguracion);
        }

        public ClienteDto BuscarPorCuit(string cuit)
        {
            Cliente cliente = repositorioClientes.BuscarPorCuit(cuit);
            return Mapper.Map<ClienteDto>(cliente);
        }

        public ClienteDto BuscarPorNumero(int numero)
        {
            Cliente cliente = repositorioClientes.BuscarPorCodigo(numero);
            return Mapper.Map<ClienteDto>(cliente);
        }

        public IEnumerable<ClienteDto> BuscarPorRazonSocial(string razonSocial, int? zona = null, int? gira = null)
        {
            IEnumerable<Cliente> clientes = repositorioClientes.BuscarPorRazonSocial(razonSocial, zona: zona, gira: gira);
            return Mapper.Map<IEnumerable<Cliente>, IEnumerable<ClienteDto>>(clientes);
        }

        public ClienteDto BuscarPorUsuario(string nombreUsuario)
        {
            if (!int.TryParse(nombreUsuario, out int numeroCliente))
            {
                return null;
            }

            return BuscarPorNumero(numeroCliente);
        }

        public PagedResultDto<ClienteMinDto> ObtenerListado(int? zona = null, int pagina = 1, int tamanoPagina = 50)
        {
            var config = PageConfig.Create(pagina, tamanoPagina);
            var clientes = repositorioClientes.Buscar(zona: zona, config: config);
            int total = repositorioClientes.CountBuscar(zona: zona);

            var res = Mapper.Map<IEnumerable<ClienteBase>, IEnumerable<ClienteMinDto>>(clientes);
            return new PagedResultDto<ClienteMinDto>(res, pagina, tamanoPagina, total);
        }

       
    }
}
 