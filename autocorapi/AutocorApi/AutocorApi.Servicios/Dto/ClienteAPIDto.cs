using System;
using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto
{
    public class ClienteAPIDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public IEnumerable<Rol> Roles { get; set; }
    }
}