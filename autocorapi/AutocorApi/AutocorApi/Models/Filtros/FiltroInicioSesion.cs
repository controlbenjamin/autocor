using System;
using System.Collections.Generic;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Models.Filtros
{
    public class FiltroInicioSesion : FiltroPaginacion
    {
        public static readonly IEnumerable<string> TiposCatalogos = new List<string>
        {
            InicioSesionDto.TipoCatalogo_Escritorio,
            InicioSesionDto.TipoCatalogo_Mobile,
            InicioSesionDto.TipoCatalogo_Web
        };

        public int? Cliente { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string TipoCatalogo { get; set; }
    }
}