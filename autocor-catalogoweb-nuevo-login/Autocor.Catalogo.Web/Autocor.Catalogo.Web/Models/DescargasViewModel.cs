using AutocorApi.Servicios.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class DescargasViewModel
    {
        public IEnumerable<DescargaDto> ListaDescarga { get; set; }
    }
}