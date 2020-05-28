using System;
using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Repositorios.Utils;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Utils;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioIniciosSesion : IServicioIniciosSesion
    {
        private IRepositorioIniciosSesion repositorioIniciosSesion;

        public ServicioIniciosSesion(IRepositorioIniciosSesion repositorioIniciosSesion)
        {
            this.repositorioIniciosSesion = repositorioIniciosSesion;
        }

        public PagedResultDto<InicioSesionDto> ObtenerCambiosUsuariosEscritorio(int? codigoCliente = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, int pagina = 1, int tamanoPagina = 50)
        {
            var config = PageConfig.Create(pagina, tamanoPagina);
            var iniciosSesion = repositorioIniciosSesion.ObtenerIniciosSesion(codigoCliente, InicioSesion.TipoCatalogo_Escritorio, fechaDesde, fechaHasta, null, true, page: config);
            int total = repositorioIniciosSesion.CountObtenerIniciosSesion(codigoCliente, InicioSesion.TipoCatalogo_Escritorio, fechaDesde, fechaHasta, null, true);

            var iniciosSesionDto = Mapper.Map<IEnumerable<InicioSesion>, IEnumerable<InicioSesionDto>>(iniciosSesion);
            return new PagedResultDto<InicioSesionDto>(iniciosSesionDto, pagina, tamanoPagina, total);
        }

        public PagedResultDto<InicioSesionDto> Buscar(int? codigoCliente = null, string tipoCatalogo = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, int pagina = 1, int tamanoPagina = 50)
        {
            var config = PageConfig.Create(pagina, tamanoPagina);
            var iniciosSesion = repositorioIniciosSesion.ObtenerIniciosSesion(codigoCliente, tipoCatalogo, fechaDesde, fechaHasta, null, false, page: config);
            int total = repositorioIniciosSesion.CountObtenerIniciosSesion(codigoCliente, tipoCatalogo, fechaDesde, fechaHasta, null, false);

            var iniciosSesionDto = Mapper.Map<IEnumerable<InicioSesion>, IEnumerable<InicioSesionDto>>(iniciosSesion);
            return new PagedResultDto<InicioSesionDto>(iniciosSesionDto, pagina, tamanoPagina, total);
        }
    }
}