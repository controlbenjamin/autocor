using AutocorApi.Servicios.Dto.Clientes;
using System.Collections.Generic;

namespace AutocorApi.Models.Clientes
{
    public class ResultadoBusquedaClientesModel
    {
        public ClienteDto PorCodigo { get; set; }
        public IEnumerable<ClienteDto> PorRazonSocial { get; set; }
    }
}